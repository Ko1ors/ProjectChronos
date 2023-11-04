using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Models;
using ProjectChronos.Common.Models.Enums;

namespace ProjectChronos.Common.Interfaces.Services
{
    public interface ICardPackService
    {
        bool EnsureWelcomePackTemplateExists();

        Task<CreatedPacks> CreatePacksAsync(int cardPackTemplateId);

        Task<CreatedPacks> CreatePacksAsync(CardPackType type);

        Task<CreatedPacks> CreatePacksAsync(ICardPackTemplate packTemplate);

        Task<BaseServiceResult> ClaimPackAsync(IUser user, CardPackType type);

        int GetPacksRemaining(CardPackType type);
    }
}
