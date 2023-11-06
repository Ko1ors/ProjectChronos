using Newtonsoft.Json;

namespace ProjectChronos.Common.Models.ExpressApi
{
    public class ExpressNft : ExpressNftBase
    {
        [JsonProperty("metadata")]
        public NftMetadata Metadata { get; set; }
    }
}
