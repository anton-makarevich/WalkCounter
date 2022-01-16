using System.Collections.Generic;
using Newtonsoft.Json;

namespace WalkCounterClient.Models
{
    public class GetWalksResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("data")]
        public List<Walk> Data { get; set; }
    }
}
