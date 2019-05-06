using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Karma.Events
{
    public class OnJoinedGuildEvent : IEvent
    {
        private readonly DiscordShardedClient _client;

        public OnJoinedGuildEvent( DiscordShardedClient client)
        {
            _client = client;
        }
        
        public void Load() => _client.JoinedGuild += guild => Task.Factory.StartNew(() => ExecuteAsync(guild));
        
        private async Task ExecuteAsync(IGuild guild)
        {
            await _client.SetGameAsync($"{_client.Guilds.Count.ToString()} Guilds!", type: ActivityType.Watching);
        }
    }
}