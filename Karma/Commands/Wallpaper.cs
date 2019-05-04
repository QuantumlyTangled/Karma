using System.Threading.Tasks;
using Discord;
using Finite.Commands;
using Karma.Core;
using Karma.Core.Attributes;
using Karma.API.NekosLife;

namespace Karma.Commands
{
    [Alias("wp", "wallpaper")]
    public class Wallpaper : SystemBase
    {
        [Command, BotPerms(ChannelPermission.AttachFiles)]
        public async Task WallpaperAsync()
        {
            var wp = await NekosLife.Client.Image_v3.Wallpaper();
            var img = NekosLife.RetrieveImage(wp.ImageUrl);
            await Context.Channel.SendFileAsync(img, "Nyaaa.png");
        }
    }
}