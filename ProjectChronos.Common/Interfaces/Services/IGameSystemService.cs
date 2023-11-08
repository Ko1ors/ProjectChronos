using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Interfaces.Services
{
    public interface IGameSystemService
    {
        public Task<IEnumerable<IOpponent>> GetOrCreateUserOpponentsAsync(IUser user);
    }
}
