using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Models.ExpressApi;
using System.Collections.Generic;

namespace ProjectChronos.Common.Interfaces.Services
{
    public interface IExpressApiService
    {
        Task<ExpressResponse<T>> SendRequestAsync<T>(string url, string method, object body = null, bool retry = true);

        Task<ExpressResponse<IEnumerable<ExpressNft>>> GetOwnedNftsAsync(string address, bool retry = true);

        Task<ExpressResponse<IEnumerable<ExpressPack>>> GetOwnedPacksAsync(string address, bool retry = true);

        Task<bool> ClaimNftsAsync(int tokenId, int amount, bool retry = true);

        Task<bool> CreatePacksAsync(ICardPackTemplate packTemplate, string internalId = "", bool retry = true);

        Task<bool> TransferPackAsync(int packId, string address, int amount, bool retry = true);
    }
}
