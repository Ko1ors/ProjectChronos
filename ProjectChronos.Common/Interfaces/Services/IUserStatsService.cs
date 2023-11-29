using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Models;

namespace ProjectChronos.Common.Interfaces.Services
{
    public interface IUserStatsService
    {
        Task<CompositeUserStats> GetCompositeUserStatsAsync(IUser user);
    }
}
