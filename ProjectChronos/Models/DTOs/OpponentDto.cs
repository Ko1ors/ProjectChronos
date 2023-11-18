namespace ProjectChronos.Models.DTOs
{
    public class OpponentDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UserDeckDto OpponentDeck { get; set; }
    }
}
