using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Models.DTOs;

namespace ProjectChronos.Extensions
{
    public static class DtoExtensions
    {
        public static DeckCardDto ToDto(this IDeckCard deckCard)
        {
            return new DeckCardDto
            {
                // Id = deckCard.Id,
                CardId = deckCard.CardId,
                Quantity = deckCard.Quantity
            };
        }

        public static IEnumerable<DeckCardDto> ToDto(this IEnumerable<IDeckCard> deckCards)
        {
            return deckCards.Select(ToDto);
        }

        public static UserDeckDto ToDto(this IUserDeck userDeck)
        {
            return new UserDeckDto
            {
                Id = userDeck.Id,
                Name = userDeck.Name,
                Active = userDeck.Active,
                Cards = userDeck.DeckCards.ToDto()
            };
        }

        public static IEnumerable<UserDeckDto> ToDto(this IEnumerable<IUserDeck> userDecks)
        {
            return userDecks.Select(ToDto);
        }

        public static OpponentDto ToDto(this IOpponent opponent)
        {
            return new OpponentDto
            {
                Id = opponent.Id,
                Name = opponent.Name,
                OpponentDeck = opponent.OpponentDeck.ToDto()
            };
        }

        public static IEnumerable<OpponentDto> ToDto(this IEnumerable<IOpponent> opponents)
        {
            return opponents.Select(ToDto);
        }
    }
}
