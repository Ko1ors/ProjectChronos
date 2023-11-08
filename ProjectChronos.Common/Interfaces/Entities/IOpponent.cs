namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IOpponent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedAt { get; set; }

        // User can be null if the opponent is a bot
        public IUser? User { get; set; }

        public int OpponentDeckId { get; set; }

        public IOpponentDeck OpponentDeck { get; set; }

        public ICollection<IUser> OpponentUsers { get; set; }
    }
}
