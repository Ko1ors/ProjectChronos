using Newtonsoft.Json;
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


        public Task<ExpressResponse<IEnumerable<ExpressNft>>> GetOwnedNftsAsync(string address, bool retry = true)
        {
            var url = $"{ExpressApiUrl}/nft/owned/{address}";
            return SendRequestAsync<IEnumerable<ExpressNft>>(url, "GET");
        }
    }
}
