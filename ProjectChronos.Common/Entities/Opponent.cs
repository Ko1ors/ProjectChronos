using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Entities
{
    public class Opponent : IOpponent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedAt { get; set; }

        // User can be null if the opponent is a bot
        public IUser? User { get; set; }

        public int OpponentDeckId { get; set; }

        public IOpponentDeck OpponentDeck { get; set; }
    }
}
