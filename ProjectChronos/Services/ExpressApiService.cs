using Newtonsoft.Json;
using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.Common.Models.ExpressApi;
using System.Text;

namespace ProjectChronos.Services
{
    public class ExpressApiService : IExpressApiService
    {
        private readonly IConfiguration Configuration;

        private HttpClient _httpClient;

        private string ExpressApiUrl => Configuration["ExpressUrl"];

        private int Retries => int.Parse(Configuration["ExpressRequestRetries"]);

        private HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                }

                return _httpClient;
            }
        }

        public ExpressApiService(IConfiguration configuration)
        {
            Configuration = configuration;

            if (string.IsNullOrEmpty(ExpressApiUrl))
            {
                throw new Exception("Express API URL is not configured");
            }
        }

        public async Task<ExpressResponse<T>> SendRequestAsync<T>(string url, string method, object body = null, bool retry = true)
        {
            try
            {
                var maxRetries = retry ? Retries : 1;
                var currentRetries = 0;
                var lastResponseContent = string.Empty;

                while (currentRetries < maxRetries)
                {
                    try
                    {
                        var request = new HttpRequestMessage(new HttpMethod(method), url);

                        if (body != null)
                        {
                            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                        }

                        var response = await HttpClient.SendAsync(request);
                        lastResponseContent = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            var responseObject = JsonConvert.DeserializeObject<T>(lastResponseContent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                            return new ExpressResponse<T>
                            {
                                Success = true,
                                Data = responseObject,
                                Message = "Success"
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!retry || currentRetries >= Retries)
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        currentRetries++;
                    }
                    // Add some delay to avoid spamming the API
                    Thread.Sleep(500 * currentRetries);
                }

                return new ExpressResponse<T>
                {
                    Success = false,
                    Message = JsonConvert.DeserializeObject<ExpressError>(lastResponseContent)?.Error ?? "Unknown error"
                };
            }
            catch (Exception ex)
            {
            }

            return new ExpressResponse<T>
            {
                Success = false,
                Message = "Unknown error"
            };
        }


        public async Task<bool> SendRequestAsync(string url, string method, object body = null, bool retry = true)
        {
            try
            {
                var request = new HttpRequestMessage(new HttpMethod(method), url);

                if (body != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                }

                var maxRetries = retry ? Retries : 1;
                var currentRetries = 0;
                var lastResponseContent = string.Empty;

                while (currentRetries < maxRetries)
                {
                    try
                    {
                        var response = await HttpClient.SendAsync(request);
                        lastResponseContent = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                            return true;
                    }
                    catch (Exception ex)
                    {
                        if (!retry || currentRetries >= Retries)
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        currentRetries++;
                    }
                    // Add some delay to avoid spamming the API
                    Thread.Sleep(500 * currentRetries);
                }
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public Task<ExpressResponse<IEnumerable<ExpressNft>>> GetAllNftsAsync(bool retry = true)
        {
            var url = $"{ExpressApiUrl}/nft/";
            return SendRequestAsync<IEnumerable<ExpressNft>>(url, "GET", retry: retry);
        }

        public Task<ExpressResponse<IEnumerable<ExpressNft>>> GetOwnedNftsAsync(string address, bool retry = true)
        {
            var url = $"{ExpressApiUrl}/nft/owned/{address}";
            return SendRequestAsync<IEnumerable<ExpressNft>>(url, "GET", retry: retry);
        }

        public Task<ExpressResponse<IEnumerable<ExpressPack>>> GetOwnedPacksAsync(string address, bool retry = true)
        {
            var url = $"{ExpressApiUrl}/packs/owned/{address}";
            return SendRequestAsync<IEnumerable<ExpressPack>>(url, "GET", retry: retry);
        }

        public Task<bool> ClaimNftsAsync(int tokenId, int amount, bool retry = true)
        {
            var url = $"{ExpressApiUrl}/nft/claim";
            var body = new
            {
                tokenId,
                amount
            };
            return SendRequestAsync(url, "POST", body, retry: retry);
        }

        public Task<bool> CreatePacksAsync(ICardPackTemplate packTemplate, string internalId = "", bool retry = true)
        {
            var url = $"{ExpressApiUrl}/packs/create";
            var body = new CreatePackRequestBody(packTemplate, internalId);
            return SendRequestAsync(url, "POST", body, retry: retry);
        }

        public Task<bool> TransferPackAsync(int packId, string address, int amount, bool retry = true)
        {
            var url = $"{ExpressApiUrl}/packs/transfer";
            var body = new
            {
                tokenId = packId,
                address,
                amount
            };
            return SendRequestAsync(url, "POST", body, retry: retry);
        }

        public Task<ExpressResponse<ExpressPackContent>> GetPackContentAsync(int packId, bool retry = true)
        {
            var url = $"{ExpressApiUrl}/packs/content/{packId}";

            return SendRequestAsync<ExpressPackContent>(url, "GET", retry: retry);
        }
    }
}
