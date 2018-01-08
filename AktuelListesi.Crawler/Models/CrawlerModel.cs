using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace AktuelListesi.Crawler.Models
{
    [JsonObject("NewsApp")]
    public class CrawlerItem
    {
        [JsonProperty("cid")]
        public string CategoryId { get; set; }
        [JsonProperty("category_name")]
        public string CategoryName { get; set; }
        [JsonProperty("category_image")]
        public string CategoryImage { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("nid")]
        public string NewsId { get; set; }
        [JsonProperty("news_heading")]
        public string NewsHeading { get; set; }
        [JsonProperty("cat_id")]
        public string NewsCategoryId { get; set; }
        [JsonProperty("news_status")]
        public string NewsStatus { get; set; }
        [JsonProperty("news_date")]
        public string NewsDate { get; set; }
        [JsonProperty("news_image")]
        public string NewsImage { get; set; }

        private string _NewsDescription;

        [JsonProperty("news_description")]
        public string NewsDescription { 
            get{
                return _NewsDescription;
            } set{
                if (Links == null) Links = new List<string>();
                _NewsDescription = value;
                if (!string.IsNullOrEmpty(value))
                {
                    Links.Clear();
                    var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    foreach (Match m in linkParser.Matches(value))
                        Links.Add(m.Value);
                }
            }
        }

        public List<string> Links { get; set; }
    }

    public class CrawlerModel
    {
        [JsonProperty("NewsApp")]
        public List<CrawlerItem> CompanyItems { get; set; }
    }
}
