namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IMatchDrawnCard
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public int MatchDrawTurnId { get; set; }

        public IMatchDrawTurn MatchDrawTurn { get; set; }
    }
}
