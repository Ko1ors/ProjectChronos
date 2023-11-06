using Newtonsoft.Json;

namespace ProjectChronos.Common.Models.ExpressApi
{
    public class ExpressPack : ExpressNftBase
    {
        [JsonProperty("metadata")]
        public PackMetadata Metadata { get; set; }
    }
}
