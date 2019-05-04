using System.Threading.Tasks;
using Finite.Commands;
using Discord;
using Discord.WebSocket;
using Karma.Core;
using Karma.Core.Attributes;
using Karma.Core.Configuration;

namespace Karma.Commands
{
    [Alias("manage", "botadmin", "root", "bot")]
    public class Manage : SystemBase
    {
        private readonly MasterConfig _config;
        private readonly DiscordShardedClient _client;
        
        public Manage(MasterConfig config, DiscordShardedClient client) {
            _config = config;
            _client = client;
        }
        
        [Command("master")]
        public async Task MasterAsync([Remainder] string action) {
            if (action == "set")
            {
                _config.AssignMasterGuild(Context.Guild.Id);
                await ReplyAsync($"It seems that the classroom has moved to a place named {Format.Bold(Context.Guild.Name)}.");
                return;
            }
            if (action == "remove")
            {
                _config.RemoveMasterGuild();
                await ReplyAsync("Everything must come to an end. But I didnt expect this. ¯\\_(ツ)_/¯");
                return;
            }
            await ReplyAsync("You really must be un-determined to not provide an action. Such a shame.");
        }
    }
}