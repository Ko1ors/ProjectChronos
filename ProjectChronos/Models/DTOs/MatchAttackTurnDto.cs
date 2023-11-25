namespace ProjectChronos.Models.DTOs
{
    public class MatchAttackTurnDto : MatchTurnDto
    {
        public int AttackCardId { get; set; }

        public int TargetCardId { get; set; }

        public bool IsEvaded { get; set; }

        public int AttackDamage { get; set; }

        public int TargetHealth { get; set; }
    }
}
