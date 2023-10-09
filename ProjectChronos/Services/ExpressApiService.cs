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

        public async Task<ExpressResponse<T>> SendRequestAsync<T>(string url, string method, object body = null)
        {
            try
            {
                var request = new HttpRequestMessage(new HttpMethod(method), url);

                if (body != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                }

                var response = HttpClient.SendAsync(request).Result;
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var responseObject = JsonConvert.DeserializeObject<T>(responseContent);
                    return new ExpressResponse<T>
                    {
                        Success = true,
                        Data = responseObject,
                        Message = "Success"
                    };
                }

                return new ExpressResponse<T>
                {
                    Success = false,
                    Message = JsonConvert.DeserializeObject<ExpressError>(responseContent)?.Error ?? "Unknown error"
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


        public Task<ExpressResponse<Metadata>> GetOwnedNftsAsync(string address)
        {
            var url = $"{ExpressApiUrl}/api/nft/owned/{address}";
            return SendRequestAsync<Metadata>(url, "GET");
        }
    }
}
