using System.Threading.Tasks;
using Finite.Commands;
using Discord;
using Discord.WebSocket;
using Karma.Core;
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
        public async Task MasterAsync() {
            _config.AssignMasterGuild(Context.Guild.Id);
            await ReplyAsync($"It seems that the class room has moved to a place named {Format.Bold(Context.Guild.Name)}.");
        }
    }
}