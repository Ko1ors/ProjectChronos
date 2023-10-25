using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Interfaces.Services
{
    public interface ICardDeckService
    {

        bool CreateCardDeck(IUser user, IEnumerable<IDeckCard> cards);

        bool UpdateCardDeck(IUser user, int deckId, IEnumerable<IDeckCard> cards);

        bool DeleteCardDeck(IUser user);
    }
}
