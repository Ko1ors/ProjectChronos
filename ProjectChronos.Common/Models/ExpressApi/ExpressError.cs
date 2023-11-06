using Newtonsoft.Json;

namespace ProjectChronos.Common.Models.ExpressApi
{
    public class ExpressError
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
