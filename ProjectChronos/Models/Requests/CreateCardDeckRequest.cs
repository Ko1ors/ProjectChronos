using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Models.Requests
{
    public class CreateCardDeckRequest
    {
        public IEnumerable<CreateDeckCard> Cards { get; set; }
    }

    public class CreateDeckCard
    {
        public int CardId { get; set; }

        public int Quantity { get; set; }

        public bool Active { get; set; }

        public IDeckCard ToDeckCard()
        {
            return new DeckCard
            {
                CardId = CardId,
                Quantity = Quantity
            };
        }
    }
}
