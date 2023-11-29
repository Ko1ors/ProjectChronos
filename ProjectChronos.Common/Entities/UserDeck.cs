using ProjectChronos.Common.Interfaces.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectChronos.Common.Entities
{
    [Table("UserDecks")]
    public class UserDeck : IUserDeck
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public IUser? User { get; set; }

        public ICollection<IDeckCard> DeckCards { get; set; }

        public int Size => DeckCards.Sum(c => c.Quantity);

        public IUserDeck Clone()
        {
            var deck = new UserDeck
            {
                Name = Name,
                Active = Active,
                User = null,
            };
            deck.DeckCards = DeckCards.Select(x => x.Clone(deck)).ToList();

            return deck;
        }
    }
}
