namespace ProjectChronos.Models.DTOs
{
    public class MatchDrawTurnDto : MatchTurnDto
    {
        public IEnumerable<MatchDrawnCardDto> Cards { get; set; }
    }
}
