namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IDeckCard
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public int Quantity { get; set; }

        public bool Active { get; set; }

        public IUserDeck UserDeck { get; set; }
    }
}
