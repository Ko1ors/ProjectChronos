using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.DB;
using System.Collections.ObjectModel;

namespace ProjectChronos.Services
{
    public class CardDeckService : ICardDeckService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IExpressApiService _expressApiService;


        public CardDeckService(ApplicationDbContext dbContext, IExpressApiService expressApiService)
        {
            _dbContext = dbContext;
            _expressApiService = expressApiService;
        }

        public bool CreateCardDeck(IUser user, IEnumerable<IDeckCard> cards)
        {
            try
            {
                var deck = new UserDeck
                {
                    User = user,
                    DeckCards = new Collection<IDeckCard>(cards.ToList()),
                };

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool DeleteCardDeck(IUser user)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCardDeck(IUser user, int deckId, IEnumerable<IDeckCard> cards)
        {
            throw new NotImplementedException();
        }
    }
}
