using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Models
{
    public class BannerFeed
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string background_image { get; set; }
        public string link { get; set; }
        public string content_id { get; set; }

        public bool is_darkText { get; set; } = false;
    }
}
