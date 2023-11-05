namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IUserDeck
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        // User can be null if the deck is an opponent deck
        public IUser? User { get; set; }

        public ICollection<IDeckCard> DeckCards { get; set; }
    }
}
