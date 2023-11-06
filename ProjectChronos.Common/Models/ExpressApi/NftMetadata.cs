using Newtonsoft.Json;

namespace ProjectChronos.Common.Models.ExpressApi
{
    public class NftMetadata : Metadata
    {
        [JsonProperty("element")]
        public string Element { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("rarity")]
        public string Rarity { get; set; }

        [JsonProperty("power")]
        public int Power { get; set; }

        [JsonProperty("health")]
        public int Health { get; set; }

        [JsonProperty("agility")]
        public int Agility { get; set; }
    }
}
