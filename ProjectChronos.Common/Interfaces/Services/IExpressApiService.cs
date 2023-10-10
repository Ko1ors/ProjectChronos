using ProjectChronos.Common.Models.ExpressApi;
using System.Collections.Generic;

namespace ProjectChronos.Common.Interfaces.Services
{
    public interface IExpressApiService
    {
        Task<ExpressResponse<T>> SendRequestAsync<T>(string url, string method, object body = null, bool retry = true);

        Task<ExpressResponse<IEnumerable<ExpressNft>>> GetOwnedNftsAsync(string address, bool retry = true);
    }
}
