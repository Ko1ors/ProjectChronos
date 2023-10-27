using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Models;

namespace ProjectChronos.Common.Interfaces.Services
{
    public interface ICardDeckService
    {

        Task<bool> CreateCardDeckAsync(IUser user, IEnumerable<IDeckCard> cards, bool active = false);

        Task<bool> UpdateCardDeckAsync(IUser user, int deckId, IEnumerable<IDeckCard> cards, bool active = false);

        bool DeleteCardDeck(IUser user, int deckId);

        bool MarkAsActive(IUser user, int deckId);
    }
}
