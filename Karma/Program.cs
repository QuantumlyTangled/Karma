using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Karma
{
    public class Program
    {
        private readonly IConfiguration _config;
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
            _config = BuildConfig();
            
            _client.Log += LogAsync;
            _client.ShardReady += ReadyAsync;
        }

        private async Task StartAsync()
        {
            _manager = new Manager(_config, _client);
            _manager.Boot();
            
            await _client.LoginAsync(TokenType.Bot, _config["token"]);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private async Task ReadyAsync(DiscordSocketClient client)
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");
            await client.SetGameAsync($"{_client.Guilds.Count.ToString()} Guilds!", null, ActivityType.Watching);
        }
        
        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }
        
    }
}