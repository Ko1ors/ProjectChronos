using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Interfaces.Services
{
    public interface ICardDeckService
    {
        UserDeck GetActiveUserDeck(IUser user);

        IEnumerable<UserDeck> GetAllUserDecks(IUser user);

        Task<bool> CreateCardDeckAsync(IUser user, IEnumerable<IDeckCard> cards, bool active = false);

        Task<bool> UpdateCardDeckAsync(IUser user, int deckId, IEnumerable<IDeckCard> cards, bool active = false);

        Task<bool> ValidateCardsOwnershipAsync(IUser user, IEnumerable<IDeckCard> cards);

        bool DeleteCardDeck(IUser user, int deckId);

        bool MarkAsActive(IUser user, int deckId);
    }
}
