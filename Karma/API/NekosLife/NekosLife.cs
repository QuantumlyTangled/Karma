using System.IO;
using System.Net.Http;
using NekosSharp;

namespace Karma.API.NekosLife
{
    public static class NekosLife
    {
        private static string _botName = "Karma";
        public static NekoClient Client { get; } = new NekoClient(_botName) { LogType = LogType.None };

        public static Stream RetrieveImage(string url)
        {
            Stream stream;
            using (var client = new HttpClient()) { stream = client.GetStreamAsync(url).GetAwaiter().GetResult(); }
            return stream;
        }
    }
}