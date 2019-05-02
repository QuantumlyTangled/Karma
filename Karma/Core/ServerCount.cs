using System;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.WebSocket;

namespace Karma.Core
{
    public class ServerCount
    {
        private readonly DiscordShardedClient _client;
        private readonly Timer _timer = new Timer(TimeSpan.FromMinutes(2).TotalMilliseconds);

        public int PreviousGuildCount { get; set; }

        public ServerCount(DiscordShardedClient client) {
            _client = client;
        }

        public void Start()
        {
            _timer.AutoReset = true;
            _timer.Elapsed += (sender, args) => Task.Factory.StartNew(() => UpdateServerCountAsync());
            _timer.Start();
            _client.SetStatusAsync(UserStatus.Online).GetAwaiter();
        }

        private void UpdateServerCountAsync() {
            if (PreviousGuildCount == _client.Guilds.Count) return;
            PreviousGuildCount = _client.Guilds.Count;
            _client.SetStatusAsync(UserStatus.Online).GetAwaiter();
            _client.SetGameAsync($"{PreviousGuildCount.ToString()} Guilds!", type:ActivityType.Watching).GetAwaiter();
        }
    }
}