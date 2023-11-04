using Newtonsoft.Json;

namespace ProjectChronos.Common.Models.ExpressApi
{
    public class ExpressPackContent
    {
        [JsonProperty("erc20Rewards")]
        public List<ExpressPackReward> Erc20Rewards { get; set; }

        [JsonProperty("erc721Rewards")]
        public List<ExpressPackReward> Erc721Rewards { get; set; }

        [JsonProperty("erc1155Rewards")]
        public List<ExpressPackReward> Erc1155Rewards { get; set; }
    }
}
