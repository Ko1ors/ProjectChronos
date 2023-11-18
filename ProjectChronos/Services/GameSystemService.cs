using Microsoft.EntityFrameworkCore;
using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.DB;
using ProjectChronos.Entities;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace ProjectChronos.Services
{
    public class GameSystemService : IGameSystemService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IExpressApiService _expressApiService;

        public const int OpponentsCount = 3;
        public const int DeckSize = 12;

        public GameSystemService(ApplicationDbContext dbContext, IExpressApiService expressApiService)
        {
            _dbContext = dbContext;
            _expressApiService = expressApiService;
        }

        public async Task<IEnumerable<IOpponent>> GetOrCreateUserOpponentsAsync(IUser user)
        {
            // If opponents exist, return them
            var opponents = _dbContext.Opponents
                .Include(o => o.OpponentUsers)
                .Include(o => o.OpponentDeck)
                .ThenInclude(d => d.DeckCards)
                .Where(o => o.OpponentUsers.Any(u => u.Id == user.Id))
                .ToList();

            if(opponents.Any())
                return opponents;

            // TODO: fetch existing active opponents

            // Else create them
            // Get all card data
            var cards = await _expressApiService.GetAllNftsAsync();
            if(!cards.Success || !cards.Data.Any())
                return Enumerable.Empty<IOpponent>();

            for (var i = 0; i < OpponentsCount; i++)
            {
                var opponentName = NameGenerator.GenerateName();

                // Create opponent
                var opponent = new Opponent
                {
                    Name = opponentName,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    OpponentUsers = new Collection<IUser>()
                    {
                        user
                    }
                };

                // Create opponent deck
                var opponentDeck = new OpponentDeck
                {
                    Name = $"{opponentName}'s deck",
                    Opponent = opponent,
                    Active = true,
                    DeckCards = new Collection<IDeckCard>()
                };


                // Fill deck with random cards
                // TODO: implement more balanced deck generation
                while (opponentDeck.DeckCards.Count < DeckSize)
                {
                    // Get random cards
                    var randomCard = cards.Data.OrderBy(d => Guid.NewGuid()).First();
     
                    // Add card to deck
                    opponentDeck.DeckCards.Add(new DeckCard
                    {
                        CardId = int.Parse(randomCard.Metadata.Id),
                        Quantity = 1,
                        UserDeck = opponentDeck
                    });
                }

                // Add deck to opponent
                opponent.OpponentDeck = opponentDeck;

                // Add opponent to db
                _dbContext.Opponents.Add(opponent);

                opponents.Add(opponent);
            }

            // Save changes
            await _dbContext.SaveChangesAsync();

            return opponents;
        }
    }
}
