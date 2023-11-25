using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Entities
{
    public class MatchTurn : IMatchTurn
    {
        public int Id { get; set; }

        public int Index { get; set; }

        public int MatchInstanceId { get; set; }

        public bool IsUserTurn { get; set; }

        public IMatchInstance MatchInstance { get; set; }
    }
}
