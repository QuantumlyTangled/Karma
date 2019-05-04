using System.Threading.Tasks;
using Discord;
using Finite.Commands;
using Karma.Core;
using Karma.Core.Attributes;
using Karma.API.NekosLife;

namespace Karma.Commands
{
    [Alias("waifu")]
    public class Waifu : SystemBase
    {
        [Command, BotPerms(ChannelPermission.AttachFiles)]
        public async Task WaifuAsync()
        {
            var waifu = await NekosLife.Client.Image_v3.Waifu();
            var img = NekosLife.RetrieveImage(waifu.ImageUrl);
            await Context.Channel.SendFileAsync(img, "Nyaaa.png");
        }
    }
}