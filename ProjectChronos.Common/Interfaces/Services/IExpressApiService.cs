using ProjectChronos.Common.Models.ExpressApi;

namespace ProjectChronos.Common.Interfaces.Services
{
    public interface IExpressApiService
    {
        Task<ExpressResponse<T>> SendRequestAsync<T>(string url, string method, object body = null);

        Task<ExpressResponse<Metadata>> GetOwnedNftsAsync(string address);
    }
}
