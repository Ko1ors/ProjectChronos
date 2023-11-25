using ProjectChronos.Common.Entities;

namespace ProjectChronos.Models
{
    public class MatchAttackTurnExtended : MatchAttackTurn
    {
        public int AttackDamage { get; set; }

        public int TargetHealth { get; set; }
    }
}
