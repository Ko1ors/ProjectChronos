using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Models.Enums;

namespace ProjectChronos.Common.Entities
{
    public class MatchInstance : IMatchInstance
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int OpponentId { get; set; }

        public MatchResultType Result { get; set; }

        public int SystemVersion { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserDeck UserDeckSnapshot { get; set; }

        public ICollection<IMatchTurn> Turns { get; set; }
    }
}
