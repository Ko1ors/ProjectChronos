using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Entities
{
    public class MatchDrawnCard : IMatchDrawnCard
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public int MatchDrawTurnId { get; set; }

        public IMatchDrawTurn MatchDrawTurn { get; set; }
    }
}
