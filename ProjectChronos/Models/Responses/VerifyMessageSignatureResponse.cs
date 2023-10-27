using Newtonsoft.Json;

namespace ProjectChronos.Models.Responses
{
    public class VerifyMessageSignatureResponse
    {
        [JsonProperty("d")]
        public VerifyMessageSignatureContent Content { get; set; }
    }
}
