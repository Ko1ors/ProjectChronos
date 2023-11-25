using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> ValidateCardsOwnershipAsync(IUser user, IEnumerable<IDeckCard> cards)
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
                    if (!ownedNfts.Data.Any(n => int.Parse(n.Metadata.Id) == card.CardId && int.Parse(n.QuantityOwned) >= card.Quantity))
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

        private IEnumerable<UserDeck> GetUserDecks(IUser user)
        {
            try
            {
                return _dbContext.UserDecks.Include(d => d.DeckCards).Where(d => d.User.Id == user.Id).AsEnumerable();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Enumerable.Empty<UserDeck>();
        }

        public async Task<bool> CreateCardDeckAsync(IUser user, IEnumerable<IDeckCard> cards, bool active = false)
        {
            try
            {
                if (!await ValidateCardsOwnershipAsync(user, cards))
                    return false;

                var userDecks = GetUserDecks(user);

                var deck = new UserDeck
                {
                    User = user,
                    Name = $"Deck #{userDecks.Count() + 1}",
                    Active = active,
                    DeckCards = new Collection<IDeckCard>(cards.ToList()),
                };

                // Mark all other decks as inactive if this one is active
                if (deck.Active)
                {
                    foreach (var userDeck in userDecks.Where(d => d.Active))
                    {
                        userDeck.Active = false;
                    }
                }

                _dbContext.Add(deck);
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
                var deckToDelete = GetUserDecks(user).FirstOrDefault(d => d.Id == deckId);
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
                var deckToUpdate = GetUserDecks(user).FirstOrDefault(d => d.Id == deckId);
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
                var deckToActivate = GetUserDecks(user).FirstOrDefault(d => d.Id == deckId);
                if (deckToActivate is null)
                    return false;
                if (deckToActivate.Active)
                    return false;

                // Mark all other decks as inactive if this one is active
                foreach (var userDeck in GetUserDecks(user).Where(d => d.Active))
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
                return GetUserDecks(user).ToList().FirstOrDefault(d => d.Active);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public IEnumerable<UserDeck> GetAllUserDecks(IUser user)
        {
            try
            {
                return GetUserDecks(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return default;
        }
    }
}
