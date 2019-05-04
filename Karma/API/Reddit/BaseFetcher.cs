using System;
using System.IO;
using System.Net.Http;
using System.Web;

namespace Karma.API.Reddit
{
    public class BaseFetcher
    {
        private readonly string _sub;

        public BaseFetcher(string subReddit)
        {
            _sub = subReddit;
        }
        
        public Stream FetchAllPosts()
        {
            Stream stream;
            var builder = new UriBuilder($"https://www.reddit.com/r/{_sub}.json");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["sort"] = "top";
            query["t"] = "week";
            query["limit"] = "800";
            builder.Query = query.ToString();
            using (var client = new HttpClient()) { stream = client.GetStreamAsync(builder.ToString()).GetAwaiter().GetResult(); }
            return stream;
        }
    }
}