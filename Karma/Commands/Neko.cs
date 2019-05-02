using System.Threading.Tasks;
using Discord;
using Finite.Commands;
using Karma.Core;
using Karma.Core.Attributes;
using Karma.API.NekosLife;

namespace Karma.Commands
{
    [Alias("neko")]
    public class Neko : SystemBase
    {
        [Command, NSFW, BotPerms(ChannelPermission.AttachFiles)]
        public async Task NekoAsync()
        {
            var neko = await NekosLife.Client.Image_v3.Neko();
            var img = NekosLife.RetrieveImage(neko.ImageUrl);
            await Context.Channel.SendFileAsync(img, "Nyaaa.png");
        }
    }
}