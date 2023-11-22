namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IMatchAttackTurn : IMatchTurn
    {
        public int AttackCardId { get; set; }

        public int TargetCardId { get; set; }

        public IMatchDrawnCard AttackCard { get; set; }

        public IMatchDrawnCard TargetCard { get; set; }
    }
}
