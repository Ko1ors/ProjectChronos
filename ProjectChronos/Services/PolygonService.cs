using ProjectChronos.Interfaces.Services;
using ProjectChronos.Models.Responses;
using Newtonsoft.Json;
using System.Text;

namespace ProjectChronos.Services
{
    public class PolygonService : IPolygonService
    {
        private HttpClient _httpClient;

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

        public string GenerateAuthMessage(string address)
        {
            return $"auth {GenerateRandomMessage()} {address}";
        }

        public async Task<bool> ValidateSignedMessageAsync(string address, string message, string signature)
        {
            // Send Post request to Polygon API to validate signature
            // https://polygonscan.com/verifiedSignatures.aspx/VerifyMessageSignature
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                address = address,
                messageRaw = message,
                messageSignature = signature,
                saveOption = "2"
            }), Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync("https://polygonscan.com/verifiedSignatures.aspx/VerifyMessageSignature", content);

            if (!response.IsSuccessStatusCode)
                return false;

            var responseContent = await response.Content.ReadAsStringAsync();
            var verifyMessageSignatureResponse = JsonConvert.DeserializeObject<VerifyMessageSignatureResponse>(responseContent);

            return verifyMessageSignatureResponse?.Content.Success ?? false;
        }

        private string GenerateRandomMessage()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
