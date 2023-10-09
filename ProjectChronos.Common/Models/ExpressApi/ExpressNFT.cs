using Newtonsoft.Json;

namespace ProjectChronos.Common.Models.ExpressApi
{
    public class ExpressNFT
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("supply")]
        public string Supply { get; set; }

        [JsonProperty("quantityOwned")]
        public string QuantityOwned { get; set; }
    }
}
