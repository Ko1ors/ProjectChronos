using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Entities
{
    public class MatchAttackTurn : MatchTurn, IMatchAttackTurn
    {
        public int AttackCardId { get; set; }

        public int TargetCardId { get; set; }

        public bool IsEvaded { get; set; }

        public IMatchDrawnCard AttackCard { get; set; }

        public IMatchDrawnCard TargetCard { get; set; }
    }
}
