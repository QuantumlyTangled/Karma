using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Karma.Core;
using Karma.Core.Configuration;

namespace Karma
{
    public class Program
    {
        private MasterConfig _config;
        private readonly DiscordShardedClient _client = new DiscordShardedClient(
            new DiscordSocketConfig
            {
                TotalShards = 1,
                LogLevel = LogSeverity.Info,
                DefaultRetryMode = RetryMode.AlwaysRetry
            }
        );

        private Manager _manager;

        static async Task Main() { await new Program().StartAsync(); }

        private Program()
        {
            
            _client.Log += LogAsync;
        }

        private async Task StartAsync()
        {
            Directories.CheckDirectories();
            _config = MasterConfig.Load();
            _manager = new Manager(_config, _client);
            _manager.Boot();
            
            await _client.LoginAsync(TokenType.Bot, _config.DiscordConfig.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }
}