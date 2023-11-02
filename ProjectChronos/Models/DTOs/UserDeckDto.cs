using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Models.DTOs
{
    public class UserDeckDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public IEnumerable<DeckCardDto> Cards { get; set; }
    }
}
