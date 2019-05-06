using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Karma.Events
{
    public class OnLeftGuildEvent : IEvent
    {
        private readonly DiscordShardedClient _client;

        public OnLeftGuildEvent( DiscordShardedClient client)
        {
            _client = client;
        }
        
        public void Load() => _client.LeftGuild += guild => Task.Factory.StartNew(() => ExecuteAsync(guild));
        
        private async Task ExecuteAsync(IGuild guild)
        {
            await _client.SetGameAsync($"{_client.Guilds.Count.ToString()} Guilds!", type: ActivityType.Watching);
        }
    }
}