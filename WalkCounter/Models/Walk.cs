using Newtonsoft.Json;

namespace WalkCounterClient.Models
{
    public class Walk
    {
        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("distance")]
        public string Distance { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }
    }
}
