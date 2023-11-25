using Microsoft.EntityFrameworkCore;
using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.Common.Models.Enums;
using ProjectChronos.DB;
using ProjectChronos.Models;
using System.Collections.ObjectModel;

namespace ProjectChronos.Services
{
    public class GameSystemService : IGameSystemService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IExpressApiService _expressApiService;
        private readonly ICardDeckService _cardDeckService;

        private readonly Random _rand;

        public const int OpponentsCount = 3;
        public const int HandSize = 5;
        public const int DeckSize = 12;
        public const int GameSystemVersion = 1;

        // TODO: move to game rules class
        private static readonly int MaxEvadeChance = 50;
        private static readonly int HalfEvadeChanceThreshold = 25;
        private static readonly float EvadeChanceMultiplier = 0.15f;

        private static readonly float StrengthMultiplier = 1.25f;
        private static readonly float WeaknessMultiplier = 0.9f;

        private static readonly Dictionary<ElementType, ElementType> ElementStrengths = new()
        {
            { ElementType.Aether, ElementType.Umbra },
            { ElementType.Umbra, ElementType.Cryo },
            { ElementType.Cryo, ElementType.Chronos },
            { ElementType.Chronos, ElementType.Aether }
        };

        private static Dictionary<ElementType, ElementType> ElementWeaknesses => ElementStrengths.ToDictionary(x => x.Value, x => x.Key);



        public GameSystemService(ApplicationDbContext dbContext, IExpressApiService expressApiService, ICardDeckService cardDeckService)
        {
            _dbContext = dbContext;
            _expressApiService = expressApiService;
            _cardDeckService = cardDeckService;

            _rand = new Random();
        }

        private IEnumerable<Opponent> GetUserOpponents(IUser user)
        {
            return _dbContext.Opponents
                .Include(o => o.OpponentUsers)
                .Include(o => o.OpponentDeck)
                .ThenInclude(d => d.DeckCards)
                .Where(o => o.OpponentUsers.Any(u => u.Id == user.Id))
                .AsEnumerable();
        }

        public async Task<IEnumerable<IOpponent>> GetOrCreateUserOpponentsAsync(IUser user)
        {
            // If opponents exist, return them
            var opponents = GetUserOpponents(user).ToList();

            if (opponents.Any())
                return opponents;

            // TODO: fetch existing active opponents

            // Else create them
            // Get all card data
            var cards = await _expressApiService.GetAllNftsAsync();
            if (!cards.Success || !cards.Data.Any())
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

        private IEnumerable<IMatchDrawnCard> DrawCards(IEnumerable<IDeckCard> deckCards)
        {
            return deckCards
                .SelectMany(card => Enumerable.Repeat(card, card.Quantity))
                .OrderBy(d => Guid.NewGuid()).Take(HandSize)
                .Select(card => new MatchDrawnCard
                {
                    CardId = card.CardId
                });
        }

        // Chance to Evade (%) = max / (1 + e^(-k * (ag - t)))
        private static double CalculateEvadeChance(MatchCardExtended card)
        {
            double expValue = -EvadeChanceMultiplier * (card.Agility - HalfEvadeChanceThreshold);
            double chance = MaxEvadeChance / (1 + Math.Pow(Math.E, expValue));
            return chance;
        }

        private bool TryEvade(MatchCardExtended card)
        {
            var chance = CalculateEvadeChance(card);
            var roll = _rand.Next(0, 100);
            return roll <= chance;
        }

        private static float CalculateModifier(MatchCardExtended attackCard, MatchCardExtended targetCard)
        {
            var modifier = 1f;
            // Check for element strength
            if (targetCard.Element == ElementStrengths[attackCard.Element])
                modifier = StrengthMultiplier;
            // Check for element weakness
            if (targetCard.Element == ElementWeaknesses[attackCard.Element])
                modifier = WeaknessMultiplier;
            return modifier;
        }

        private static int CalculateDamage(MatchCardExtended attackCard, MatchCardExtended targetCard)
        {
            return (int)Math.Round(attackCard.Power * CalculateModifier(attackCard, targetCard));
        }

        private (MatchCardExtended, MatchCardExtended) FindBestCardPair(IEnumerable<MatchCardExtended> attackCards, IEnumerable<MatchCardExtended> targetCards)
        {
            MatchCardExtended bestAttackCard = null;
            MatchCardExtended bestTargetCard = null;

            var bestScore = 0;
            foreach (var a in attackCards)
            {
                foreach (var t in targetCards)
                {
                    // Calculate score using formula: min(dmg * evadeChance, targetHealth)
                    var score = (int)Math.Round(Math.Min(CalculateDamage(a, t) * CalculateEvadeChance(t), t.CurrentHealth));
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestAttackCard = a;
                        bestTargetCard = t;
                    }
                }
            }
            return (bestAttackCard, bestTargetCard);
        }

        public async Task<IMatchInstance> InitiateMatchAsync(IUser user, int opponentId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var opponent = GetUserOpponents(user).Where(o => o.Id == opponentId).FirstOrDefault();

                if (opponent is null)
                    throw new Exception("Opponent not found");

                // Validate that user has active deck with enough cards
                var userDeck = _cardDeckService.GetActiveUserDeck(user);

                if (userDeck is null)
                    throw new Exception("User deck not found");

                if (userDeck.Size < DeckSize)
                    throw new Exception("User deck has not enough cards");

                // TODO: Validate deck's cards ownership
                var isDeckValid = await _cardDeckService.ValidateCardsOwnershipAsync(user, userDeck.DeckCards);

                if (!isDeckValid)
                    throw new Exception("User deck has invalid cards");

                // Create user deck snapshot

                var userDeckSnapshot = userDeck.Clone();

                var match = new MatchInstance
                {
                    User = user,
                    Opponent = opponent,
                    Result = MatchResultType.Pending,
                    SystemVersion = GameSystemVersion,
                    UserDeckSnapshot = userDeckSnapshot,
                    Turns = new Collection<IMatchTurn>(),
                    CreatedAt = DateTime.UtcNow
                };

                // Draw cards for user and opponent
                var userDrawnCards = DrawCards(userDeck.DeckCards);

                var opponentDrawnCards = DrawCards(opponent.OpponentDeck.DeckCards);

                // Get cards metadata
                var cards = await _expressApiService.GetAllNftsAsync();

                if (!cards.Success || !cards.Data.Any())
                    throw new Exception("Cards not found");

                // Create draw turns
                var userDrawTurn = new MatchDrawTurn
                {
                    MatchInstance = match,
                    Index = match.Turns.Count + 1,
                    IsUserTurn = true,
                    Cards = new Collection<IMatchDrawnCard>(userDrawnCards.ToList())
                };

                match.Turns.Add(userDrawTurn);

                var opponentDrawTurn = new MatchDrawTurn
                {
                    MatchInstance = match,
                    Index = match.Turns.Count + 1,
                    IsUserTurn = false,
                    Cards = new Collection<IMatchDrawnCard>(opponentDrawnCards.ToList())
                };

                match.Turns.Add(opponentDrawTurn);

                // Save interim match data here to access drawn card ids
                _dbContext.Matches.Add(match);
                _dbContext.SaveChanges();

                var userDrawnExtendedCards = match.Turns
                    .OfType<MatchDrawTurn>()
                    .First(t => t.IsUserTurn).Cards
                    .GroupJoin(cards.Data,
                        c => c.CardId,
                        c => int.Parse(c.Metadata.Id),
                        (c, d) => new MatchCardExtended(c, d.FirstOrDefault()?.Metadata)
                    ).ToList();

                var opponentDrawnExtendedCards = match.Turns
                    .OfType<MatchDrawTurn>()
                    .First(t => !t.IsUserTurn).Cards
                    .GroupJoin(cards.Data,
                        c => c.CardId,
                        c => int.Parse(c.Metadata.Id),
                        (c, d) => new MatchCardExtended(c, d.FirstOrDefault()?.Metadata)
                    ).ToList();

                if (userDrawnExtendedCards.Count != HandSize || opponentDrawnExtendedCards.Count != HandSize)
                    throw new Exception("Cards not found");

                if (userDrawnExtendedCards.Any(c => c.Metadata is null) || opponentDrawnExtendedCards.Any(c => c.Metadata is null))
                    throw new Exception("Cards not found");

                var isUserTurn = true;

                var isMatchOver = false;

                // Time to play
                while (!isMatchOver)
                {
                    IEnumerable<MatchCardExtended> attackerCards;
                    IEnumerable<MatchCardExtended> targetCards;

                    if (isUserTurn)
                    {
                        attackerCards = userDrawnExtendedCards;
                        targetCards = opponentDrawnExtendedCards;
                    }
                    else
                    {
                        attackerCards = opponentDrawnExtendedCards;
                        targetCards = userDrawnExtendedCards;
                    }

                    // Filter out non-eligible cards (only alive cards and not used last 3 turns)
                    var lastUsedCards = match.Turns
                        .OfType<MatchAttackTurn>()
                        .Where(t => t.IsUserTurn == isUserTurn)
                        .Take(3)
                        .Select(t => t.AttackCardId)
                        .ToList();

                    var attackerCardsTemp = attackerCards.Where(c => c.IsAlive && !lastUsedCards.Contains(c.Card.Id));

                    // If no cards found, try to use all alive cards
                    if (!attackerCardsTemp.Any())
                    {
                        attackerCardsTemp = attackerCards.Where(c => c.IsAlive);
                    }

                    attackerCards = attackerCardsTemp;
                    targetCards = targetCards.Where(c => c.IsAlive);

                    var bestCards = FindBestCardPair(attackerCards, targetCards);
                    var isEvaded = TryEvade(bestCards.Item2);
                    int damage = 0;

                    // If attack was not evaded, calculate damage
                    if (!isEvaded)
                    {
                        damage = CalculateDamage(bestCards.Item1, bestCards.Item2);
                        // Apply damage
                        bestCards.Item2.CurrentHealth -= damage;
                        // Check if all target cards are dead
                        if (!targetCards.Any(c => c.IsAlive))
                        {
                            isMatchOver = true;
                        }
                    }

                    var attackTurn = new MatchAttackTurnExtended
                    {
                        MatchInstance = match,
                        Index = match.Turns.Count + 1,
                        IsUserTurn = isUserTurn,
                        AttackCardId = bestCards.Item1.Card.Id,
                        TargetCardId = bestCards.Item2.Card.Id,
                        IsEvaded = isEvaded,
                        AttackDamage = damage,
                        TargetHealth = bestCards.Item2.CurrentHealth,
                    };

                    match.Turns.Add(attackTurn);

                    isUserTurn = !isUserTurn;
                }

                // TODO: Delete user opponents relationship 

                // TODO: Create new opponent based on user

                var lastTurn = match.Turns.Last();
                match.Result = lastTurn.IsUserTurn ? MatchResultType.Win : MatchResultType.Loss;
                _dbContext.SaveChanges();

                transaction.Commit();
                return match;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                transaction.Rollback();
            }
            return null;
        }
    }
}
