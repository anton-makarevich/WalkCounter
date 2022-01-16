using Newtonsoft.Json;

namespace WalkCounterClient.Models
{
    public class SaveWalkResponse
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
