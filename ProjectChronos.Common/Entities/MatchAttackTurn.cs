using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Entities
{
    internal class MatchAttackTurn : IMatchAttackTurn
    {
        public int Id { get; set; }

        public int Index { get; set; }

        public int MatchInstanceId { get; set; }

        public bool IsUserTurn { get; set; }

        public IMatchInstance MatchInstance { get; set; }

        public int AttackCardId { get; set; }

        public int TargetCardId { get; set; }

        public IMatchDrawnCard AttackCard { get; set; }

        public IMatchDrawnCard TargetCard { get; set; }
    }
}
