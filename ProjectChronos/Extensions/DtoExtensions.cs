using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Models.Enums;
using ProjectChronos.Models;
using ProjectChronos.Models.DTOs;
using System.Linq;

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

        public static MatchDrawnCardDto ToDto(this IMatchDrawnCard matchDrawnCard)
        {
            return new MatchDrawnCardDto
            {
                Id = matchDrawnCard.Id,
                CardId = matchDrawnCard.CardId,
            };
        }

        public static IEnumerable<MatchDrawnCardDto> ToDto(this IEnumerable<IMatchDrawnCard> matchDrawnCards)
        {
            return matchDrawnCards.Select(ToDto);
        }

        public static MatchTurnDto ToDto(this IMatchTurn matchTurn)
        {
            if(matchTurn is IMatchDrawTurn)
                return (matchTurn as IMatchDrawTurn).ToDto();
            if(matchTurn is MatchAttackTurnExtended)
                return (matchTurn as MatchAttackTurnExtended).ToDto();
            return new MatchTurnDto
            {
                Index = matchTurn.Index,
                IsUserTurn = matchTurn.IsUserTurn,
                TurnType = MatchTurnType.Unknown
            };
        }

        public static IEnumerable<MatchTurnDto> ToDto(this IEnumerable<IMatchTurn> matchTurns)
        {
            return matchTurns.Select(ToDto);
        }


        public static MatchDrawTurnDto ToDto(this IMatchDrawTurn matchDrawTurn)
        {
            return new MatchDrawTurnDto
            {
                Index = matchDrawTurn.Index,
                IsUserTurn = matchDrawTurn.IsUserTurn,
                TurnType = MatchTurnType.Draw,
                Cards = matchDrawTurn.Cards.ToDto()
            };
        }

        public static IEnumerable<MatchDrawTurnDto> ToDto(this IEnumerable<IMatchDrawTurn> matchDrawTurns)
        {
            return matchDrawTurns.Select(ToDto);
        }

        public static MatchAttackTurnDto ToDto(this MatchAttackTurnExtended matchAttackTurn)
        {
            return new MatchAttackTurnDto
            {
                Index = matchAttackTurn.Index,
                IsUserTurn = matchAttackTurn.IsUserTurn,
                TurnType = MatchTurnType.Attack,
                AttackCardId = matchAttackTurn.AttackCardId,
                TargetCardId = matchAttackTurn.TargetCardId,
                IsEvaded = matchAttackTurn.IsEvaded,
                AttackDamage = matchAttackTurn.AttackDamage,
                TargetHealth = matchAttackTurn.TargetHealth
            };
        }

        public static IEnumerable<MatchAttackTurnDto> ToDto(this IEnumerable<MatchAttackTurnExtended> matchAttackTurns)
        {
            return matchAttackTurns.Select(ToDto);
        }

        public static MatchDto ToDto(this IMatchInstance matchInstance)
        {
            return new MatchDto
            {
                Id = matchInstance.Id,
                UserId = matchInstance.UserId,
                OpponentId = matchInstance.OpponentId,
                Result = matchInstance.Result,
                SystemVersion = matchInstance.SystemVersion,
                CreatedAt = matchInstance.CreatedAt,
                UserDeck = matchInstance.UserDeckSnapshot.ToDto(),
                Turns = matchInstance.Turns.ToDto()
            };
        }
    }
}
