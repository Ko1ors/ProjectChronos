using ProjectChronos.Common.Models.Enums;

namespace ProjectChronos.Models.DTOs
{
    public class MatchTurnDto
    {
        public int Index { get; set; }

        public bool IsUserTurn { get; set; }

        public MatchTurnType TurnType { get; set; }
    }
}
