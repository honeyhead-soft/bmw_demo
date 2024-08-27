using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Models
{
    public class CompanyCollection
    {
        [JsonProperty("companies")]
        public List<Company> Companies { get; set; }
    }

    public class Company
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("industry_tag")]
        public IndustryTag IndustryTag { get; set; }

        [JsonProperty("company_tags")]
        public List<CompanyTag> CompanyTags { get; set; }

        [JsonProperty("title_img")]
        public CompanyImage TitleImg { get; set; }

        [JsonProperty("logo_img")]
        public CompanyImage LogoImg { get; set; }

        [JsonProperty("application_response_stats")]
        public ApplicationResponseStats ApplicationResponseStats { get; set; }

        [JsonProperty("is_follow")]
        public bool IsFollow { get; set; }

        [JsonProperty("is_joined")]
        public bool IsJoined { get; set; }

        [JsonProperty("kreditjob_id")]
        public string KreditjobId { get; set; }
    }

    public class IndustryTag
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class CompanyTag
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class CompanyImage
    {
        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }

    public class ApplicationResponseStats
    {
        [JsonProperty("avg_day")]
        public object AvgDay { get; set; }

        [JsonProperty("avg_rate")]
        public double? AvgRate { get; set; }

        [JsonProperty("delayed_count")]
        public int DelayedCount { get; set; }

        [JsonProperty("remained_count")]
        public int RemainedCount { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }
    }
}
