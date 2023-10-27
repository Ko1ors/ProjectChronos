namespace ProjectChronos.Models.Requests
{
    public class UpdateCardDeckRequest
    {
        public int DeckId { get; set; }

        public IEnumerable<CreateDeckCard> Cards { get; set; }

        public bool Active { get; set; }
    }
}
