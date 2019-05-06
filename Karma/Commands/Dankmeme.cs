using System;
using System.Threading.Tasks;
using Discord;
using Finite.Commands;
using Karma.API.NekosLife;
using Karma.API.Reddit;
using Karma.Core;
using Karma.Core.Attributes;

namespace Karma.Commands
{
    [Alias("dankmeme", "dankmemes")]
    public class Dankmeme : SystemBase
    {
        private BaseFetcher _reddit;
        private Random _rand;
        
        public Dankmeme()
        {
            _reddit = new BaseFetcher("dankmemes");
            _rand = new Random();
        }
        
        [Command, BotPerms(ChannelPermission.AttachFiles)]
        public async Task DankmemeAsync()
        {
            var all = _reddit.FetchAllPosts();
            var url = ReRunCheck(all);
            var img = NekosLife.RetrieveImage(url);
            await Context.Channel.SendFileAsync(img, "MemeIsDaDankest.png");
        }

        public string ReRunCheck(Response res)
        {
            var len = res.Data.Children.Length;
            var url = res.Data.Children[_rand.Next(len)].Data.Url;
            if (url.StartsWith("https://www.reddit.com")) url = ReRunCheck(res);
            return url;
        }
    }
}