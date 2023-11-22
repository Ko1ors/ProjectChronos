using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Entities
{
    internal class MatchDrawTurn : IMatchDrawTurn
    {
        public int Id { get; set; }

        public int Index { get; set; }

        public int MatchInstanceId { get; set; }

        public bool IsUserTurn { get; set; }

        public IMatchInstance MatchInstance { get; set; }

        public ICollection<IMatchDrawnCard> Cards { get; set; }
    }
}
