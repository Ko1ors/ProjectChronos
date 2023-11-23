using ProjectChronos.Common.Entities;

namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IUserDeck
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        // User can be null if the deck is an opponent deck
        // or if the deck is a snapshot of a user deck
        public IUser? User { get; set; }

        public ICollection<IDeckCard> DeckCards { get; set; }

        IUserDeck Clone();
    }
}
