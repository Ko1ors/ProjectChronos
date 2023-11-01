using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.DB;
using ProjectChronos.Entities;
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

        private async Task<bool> ValidateCardsOwnershipAsync(IUser user, IEnumerable<IDeckCard> cards)
        {
            try
            {
                var identityUser = (User)user;
                if (identityUser is null)
                    return false;

                var ownedNfts = await _expressApiService.GetOwnedNftsAsync(identityUser.UserName);
                foreach (var card in cards)
                {
                    // Check if nft is owned
                    if (!ownedNfts.Data.Any(n => int.Parse(n.Metadata.Id) == card.CardId && int.Parse(n.QuantityOwned) == card.Quantity))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public async Task<bool> CreateCardDeckAsync(IUser user, IEnumerable<IDeckCard> cards, bool active = false)
        {
            try
            {
                if (!await ValidateCardsOwnershipAsync(user, cards))
                    return false;

                var deck = new UserDeck
                {
                    User = user,
                    Active = active,
                    DeckCards = new Collection<IDeckCard>(cards.ToList()),
                };

                // Mark all other decks as inactive if this one is active
                if (deck.Active)
                {
                    foreach (var userDeck in user.UserDecks.Where(d => d.Active))
                    {
                        userDeck.Active = false;
                    }
                }

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool DeleteCardDeck(IUser user, int deckId)
        {
            try
            {
                var deckToDelete = user.UserDecks.FirstOrDefault(d => d.Id == deckId) as UserDeck;
                if (deckToDelete is null)
                    return false;

                _dbContext.UserDecks.Remove(deckToDelete);

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }


        public async Task<bool> UpdateCardDeckAsync(IUser user, int deckId, IEnumerable<IDeckCard> cards, bool active = false)
        {
            try
            {
                var deckToUpdate = user.UserDecks.FirstOrDefault(d => d.Id == deckId) as UserDeck;
                if (deckToUpdate is null)
                    return false;
                if (!await ValidateCardsOwnershipAsync(user, cards))
                    return false;
                deckToUpdate.DeckCards = new Collection<IDeckCard>(cards.ToList());

                if (active)
                    MarkAsActive(user, deckId);
                else
                    deckToUpdate.Active = false;

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool MarkAsActive(IUser user, int deckId)
        {
            try
            {
                var deckToActivate = user.UserDecks.FirstOrDefault(d => d.Id == deckId) as UserDeck;
                if (deckToActivate is null)
                    return false;
                if (deckToActivate.Active)
                    return false;

                // Mark all other decks as inactive if this one is active
                foreach (var userDeck in user.UserDecks.Where(d => d.Active))
                {
                    userDeck.Active = false;
                }
                deckToActivate.Active = true;

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public UserDeck GetActiveUserDeck(IUser user)
        {
            try
            {
                return user.UserDecks.FirstOrDefault(d => d.Active) as UserDeck;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return default;
        }

        public IEnumerable<UserDeck> GetAllUserDecks(IUser user)
        {
            try
            {
                return (IEnumerable<UserDeck>)user.UserDecks.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return default;

        }
    }
}
