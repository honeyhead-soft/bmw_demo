using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MauiWanted.Models
{


    public class JobPosition
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("score")]
        public double? Score { get; set; }

        [JsonProperty("due_time")]
        public string DueTime { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("action_type")]
        public object ActionType { get; set; }

        [JsonProperty("is_like")]
        public bool IsLike { get; set; }

        [JsonProperty("is_bookmark")]
        public bool IsBookmark { get; set; } = false;

        [JsonProperty("like_count")]
        public int LikeCount { get; set; }

        [JsonProperty("address")]
        public JobAddress Address { get; set; }

        [JsonProperty("title_img")]
        public JobImage TitleImg { get; set; }

        [JsonProperty("logo_img")]
        public JobImage LogoImg { get; set; }

        [JsonProperty("category_tags")]
        public List<JobCategoryTag> CategoryTags { get; set; }

        [JsonProperty("company")]
        public JobCompany Company { get; set; }

        [JsonProperty("reward")]
        public JobReward Reward { get; set; }

        [JsonProperty("has_analysis")]
        public bool HasAnalysis { get; set; }
    }

    public class JobAddress
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("full_location")]
        public string FullLocation { get; set; }
    }

    public class JobImage
    {
        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }

    public class JobCategoryTag
    {
        [JsonProperty("parent_tag")]
        public JobParentTag ParentTag { get; set; }

        [JsonProperty("child_tags")]
        public List<JobChildTag> ChildTags { get; set; }
    }

    public class JobParentTag
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class JobChildTag
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class JobCompany
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("industry_name")]
        public string IndustryName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("application_response_stats")]
        public JobApplicationResponseStats ApplicationResponseStats { get; set; }
    }

    public class JobApplicationResponseStats
    {
        [JsonProperty("avg_day")]
        public object AvgDay { get; set; }

        [JsonProperty("avg_rate")]
        public double AvgRate { get; set; }

        [JsonProperty("delayed_count")]
        public int DelayedCount { get; set; }

        [JsonProperty("remained_count")]
        public int RemainedCount { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }
    }

    public class JobReward
    {
        [JsonProperty("formatted_total")]
        public string FormattedTotal { get; set; }

        [JsonProperty("formatted_recommender")]
        public string FormattedRecommender { get; set; }

        [JsonProperty("formatted_recommendee")]
        public string FormattedRecommendee { get; set; }
    }

}
