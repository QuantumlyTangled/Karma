using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Karma.API.Reddit
{
    public class BaseFetcher
    {
        private readonly string _sub;

        public BaseFetcher(string subReddit)
        {
            _sub = subReddit;
        }
        
        public Response FetchAllPosts()
        {
            string jsonString;
            using(var client = new HttpClient())
            {
                jsonString = client.GetStringAsync($"https://www.reddit.com/r/{_sub}.json?limit=800").GetAwaiter().GetResult();
            }
            return JsonConvert.DeserializeObject<Response>(jsonString);
        }
    }
}