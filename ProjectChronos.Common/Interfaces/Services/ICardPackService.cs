using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Models.Enums;

namespace ProjectChronos.Common.Interfaces.Services
{
    public interface ICardPackService
    {
        bool EnsureWelcomePackTemplateExists();

        int CreatePacks(int cardPackTemplateId);

        int CreatePacks(CardPackType type);

        int CreatePacks(ICardPackTemplate packTemplate);

        int GetPacksRemaining(CardPackType type);
    }
}
