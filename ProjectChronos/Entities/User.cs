using Microsoft.AspNetCore.Identity;
using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Entities
{
    public class User : IdentityUser, IUser
    {
    }

}
