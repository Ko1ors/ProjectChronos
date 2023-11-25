namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IMatchDrawTurn : IMatchTurn
    {
        public ICollection<IMatchDrawnCard> Cards { get; set; }
    }
}
