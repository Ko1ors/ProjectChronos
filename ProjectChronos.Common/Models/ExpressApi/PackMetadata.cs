using Newtonsoft.Json;

namespace ProjectChronos.Common.Models.ExpressApi
{
    public class PackMetadata : Metadata
    {
        [JsonProperty("internalId")]
        public string InternalId { get; set; }
    }
}
