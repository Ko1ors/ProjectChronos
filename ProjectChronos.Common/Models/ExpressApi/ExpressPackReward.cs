using Newtonsoft.Json;

namespace ProjectChronos.Common.Models.ExpressApi
{
    public class ExpressPackReward
    {
        [JsonProperty("contractAddress")]
        public string ContractAddress { get; set; }

        [JsonProperty("tokenId")]
        public string TokenId { get; set; }

        [JsonProperty("quantityPerReward")]
        public string QuantityPerReward { get; set; }

        [JsonProperty("totalRewards")]
        public string TotalRewards { get; set; }
    }
}
