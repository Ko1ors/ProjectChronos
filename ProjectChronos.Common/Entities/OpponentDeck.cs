using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Entities
{
    public class OpponentDeck : UserDeck, IOpponentDeck
    {
        public IOpponent Opponent { get; set; }
    }
}
