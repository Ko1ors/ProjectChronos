﻿using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Models;

namespace ProjectChronos.Common.Interfaces.Services
{
    public interface IGameSystemService
    {
        public Task<IEnumerable<IOpponent>> GetOrCreateUserOpponentsAsync(IUser user);

        public Task<ServiceResult<IMatchInstance>> InitiateMatchAsync(IUser user, int opponentId);

        public IEnumerable<IMatchInstance> GetAllUserMatches(IUser user, bool includeDetails = false);
    }
}
