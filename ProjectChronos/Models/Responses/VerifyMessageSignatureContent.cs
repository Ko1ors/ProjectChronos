using Newtonsoft.Json;

namespace ProjectChronos.Models.Responses
{
    public class VerifyMessageSignatureContent
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("verifyResult")]
        public string VerifyResult { get; set; }

        [JsonProperty("saveResult")]
        public string SaveResult { get; set; }

        [JsonProperty("isDuplicatedMessage")]
        public bool IsDuplicatedMessage { get; set; }

        [JsonProperty("verifiedMessageLocation")]
        public string VerifiedMessageLocation { get; set; }
    }
}
