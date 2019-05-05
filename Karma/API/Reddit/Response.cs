using Newtonsoft.Json;

namespace Karma.API.Reddit
{
    public class Response
    {
        [JsonProperty("kind")]
        public string Kind { get; set;  }
        [JsonProperty("data")]
        public RedditResponseData Data { get; set;  }
    }

    public class RedditResponseData
    {
        [JsonProperty("modhash")]
        public string Modhash { get; set;  }
        [JsonProperty("dist")]
        public int Dist { get; set;  }
        [JsonProperty("children")]
        public RedditResponseChild[] Children { get; set;  }
    }

    public class RedditResponseChild
    {
        [JsonProperty("kind")]
        public string Kind { get; set;  }
        [JsonProperty("data")]
        public RedditResponseChildData Data { get; set;  }
    }
    
    public class RedditResponseChildData
    {
        [JsonProperty("subreddit")]
        public string Subreddit { get; set; }
        [JsonProperty("thumbnail_height")]
        public int? ThumbnailHeight { get; set; }
        [JsonProperty("thumbnail_width")]
        public int? ThumbnailWidth { get; set; }
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; } = "self";
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}