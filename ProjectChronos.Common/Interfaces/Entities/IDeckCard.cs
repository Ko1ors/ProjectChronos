using ProjectChronos.Common.Entities;

namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IDeckCard
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public int Quantity { get; set; }

        public IUserDeck UserDeck { get; set; }

        IDeckCard Clone(UserDeck deck = null);
    }
}
