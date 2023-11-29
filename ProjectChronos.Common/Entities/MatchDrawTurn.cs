using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Entities
{
    public class MatchDrawTurn : MatchTurn, IMatchDrawTurn
    {
        public ICollection<IMatchDrawnCard> Cards { get; set; }
    }
}
