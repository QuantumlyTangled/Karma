using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Karma.Core;
using Karma.Core.Configuration;

namespace Karma.Events
{
    public class OnReadyEvent : IEvent
    {
        private readonly DiscordShardedClient _client;
        private readonly ServerCount _count;
        
        private bool _initialized;
        private readonly Dictionary<int, bool> _shardsReady = new Dictionary<int, bool>();
        
        public OnReadyEvent( DiscordShardedClient client, ServerCount serverCount)
        {
            _client = client;
            _count = serverCount;
        }
        
        public void Load() => _client.ShardReady += socketClient => Task.Factory.StartNew(() => ExecuteAsync(socketClient));
        
        private async Task ExecuteAsync(DiscordSocketClient sClient)
        {
            if (!_shardsReady.ContainsKey(sClient.ShardId)) _shardsReady.Add(sClient.ShardId, false);
            else _shardsReady[sClient.ShardId] = true;

            if (_initialized) return;
            if (_shardsReady.Count < _client.Shards.Count) return;

            _initialized = true;
            _count.PreviousGuildCount = _client.Guilds.Count;
            
            Console.WriteLine($"[{sClient.ShardId.ToString()}] {_client.CurrentUser} is connected!");

            await _client.SetGameAsync($"{_client.Guilds.Count.ToString()} Guilds!", type: ActivityType.Watching);
            await _client.SetStatusAsync(UserStatus.Online);
        }
    }
}