using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Models
{
    public class ThemeZip
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("logos")]
        public List<string> Logos { get; set; }

        [JsonProperty("more_logos")]
        public string MoreLogos { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("data_theme_id")]
        public string DataThemeId { get; set; }
    }
}
