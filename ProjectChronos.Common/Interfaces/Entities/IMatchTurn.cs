namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IMatchTurn
    {
        public int Id { get; set; }

        public int Index { get; set; }

        public int MatchInstanceId { get; set; }

        public bool IsUserTurn { get; set; }

        public IMatchInstance MatchInstance { get; set; }
    }
}
