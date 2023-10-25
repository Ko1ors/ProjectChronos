using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Entities
{
    public class DeckCard : IDeckCard
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public int Quantity { get; set; }

        public IUserDeck UserDeck { get; set; }
    }
}
