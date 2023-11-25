using Microsoft.AspNetCore.Identity;
using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Entities
{
    public class User : IdentityUser, IUser
    {
        public ICollection<IUserDeck> UserDecks { get; set; }

        public ICollection<IOpponent> Opponents { get; set; }

        public ICollection<IMatchInstance> Matches { get; set; }
    }
}
