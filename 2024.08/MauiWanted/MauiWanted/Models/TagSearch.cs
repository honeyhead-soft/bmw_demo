using Newtonsoft.Json;

namespace MauiWanted.Models
{
    public class TagSearch
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("related_positions")]
        public int RelatedPositions { get; set; }

        [JsonProperty("companies")]
        public int Companies { get; set; }

        [JsonProperty("background_color")]
        public string CardHexColor { get; set; }
    }
}
