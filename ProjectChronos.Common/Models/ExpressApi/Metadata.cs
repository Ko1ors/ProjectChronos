using Newtonsoft.Json;

namespace ProjectChronos.Common.Models.ExpressApi
{
    public abstract class Metadata
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
