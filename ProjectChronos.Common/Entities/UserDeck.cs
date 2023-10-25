using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Entities
{
    public class UserDeck : IUserDeck
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public IUser User { get; set; }

        public ICollection<IDeckCard> DeckCards { get; set; }
    }
}
