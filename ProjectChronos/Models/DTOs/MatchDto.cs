using ProjectChronos.Common.Models.Enums;

namespace ProjectChronos.Models.DTOs
{
    public class MatchDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int OpponentId { get; set; }

        public MatchResultType Result { get; set; }

        public int SystemVersion { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserDeckDto UserDeck { get; set; }

        public IEnumerable<MatchTurnDto> Turns { get; set; }
    }
}
