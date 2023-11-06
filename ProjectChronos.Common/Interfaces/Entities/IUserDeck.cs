namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IUserDeck
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public IUser User { get; set; }

        public ICollection<IDeckCard> DeckCards { get; set; }
    }
}
