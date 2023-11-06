using ProjectChronos.Common.Interfaces.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectChronos.Common.Entities
{
    [Table("DeckCards")]
    public class DeckCard : IDeckCard
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public int Quantity { get; set; }

        public IUserDeck UserDeck { get; set; }
    }
}
