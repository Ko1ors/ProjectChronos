using ProjectChronos.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ProjectChronos.Entities
{
    public class User : IdentityUser, IUser
    {
    }

}
