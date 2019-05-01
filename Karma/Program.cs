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
        private readonly DiscordShardedClient _client;
        private readonly IConfiguration _config;

        // Discord.Net heavily utilizes TAP for async, so we create
        // an asynchronous context from the beginning.
        static async Task Main() { await new Program().MainAsync(); }

        private Program()
        {
            _config = BuildConfig();
            _client = new DiscordShardedClient(
                new DiscordSocketConfig
                {
                    TotalShards = 1,
                    LogLevel = LogSeverity.Info,
                    DefaultRetryMode = RetryMode.AlwaysRetry
                }
            );

            _client.Log += LogAsync;
            _client.ShardReady += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;
        }

        private async Task MainAsync()
        {
            // Tokens should be considered secret data, and never hard-coded.
            await _client.LoginAsync(TokenType.Bot, _config["token"]);
            await _client.StartAsync();

            // Block the program until it is closed.
            await Task.Delay(-1);
        }

        private static Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        // The Ready event indicates that the client has opened a
        // connection and it is now safe to access the cache.
        private async Task ReadyAsync(DiscordSocketClient client)
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");
            
            await client.SetGameAsync($"{_client.Guilds.Count.ToString()} Guilds!", null, ActivityType.Watching);
        }

        // This is not the recommended way to write a bot - consider
        // reading over the Commands Framework sample.
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id) return;

            if (message.Content == "!ping") await message.Channel.SendMessageAsync("pong!");
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