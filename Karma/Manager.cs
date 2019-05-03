using System;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Finite.Commands;
using Finite.Commands.Extensions;
using Karma.Core;
using Karma.Core.Factories;
using Karma.Core.TypeReaders;
using Karma.Core.Pipelines;
using Karma.Core.Parsers;
using Karma.Core.Configuration;
using Karma.Events;
using Karma.Events.OnMessageEvents;
using Microsoft.Extensions.DependencyInjection;

namespace Karma
{
    public class Manager
    {
        private readonly MasterConfig _config;
        private readonly DiscordShardedClient _client;
        
        private BotLog _botLog;
        private ServerCount _serverCount;
        private CommandService<SystemContext> _commandService;
        private EventLoader _eventLoader;
        private Analytics _analytics;
        private IServiceProvider _serviceProvider;
        
        public Manager(MasterConfig config, DiscordShardedClient client)
        {
            _config = config;
            _client = client;
        }

        public void Boot()
        {
            _commandService = new CommandServiceBuilder<SystemContext>()
                .AddModules(Assembly.GetEntryAssembly())
                .AddTypeReaderFactory(() => new DiscordTypeReaderFactory()
                    .AddReader(new UserTypeReader())
                )
                .AddPipeline<PrefixPipeline>()
                .AddCommandParser<SystemCommandParser<SystemContext>>()
                .AddPipeline<PreconditionPipeline>()
                .AddPipeline<FinalizePipeline>()
                .BuildCommandService();

            _botLog = new BotLog(_config, _client);
            _serverCount = new ServerCount(_client);
            _analytics = new Analytics(_botLog);
            
            _serviceProvider = new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton(_client)
                .AddSingleton(_botLog)
                .AddSingleton(_serverCount)
                .AddSingleton(_analytics)
                .AddSingleton(_commandService)
                .AddSingleton<IDiscordClient>(_client)
                .BuildServiceProvider();
            
            _eventLoader = new EventLoader()
                .LoadEvent(new OnReadyEvent(_client, _serverCount))
                .LoadEvent(new OnMessageReceivedEvent(_client)
                    .AddSubEvent(new OnCommandSubEvent(_client, _commandService, _analytics, _botLog, _serviceProvider))
                );
            
            _serverCount.Start();
        }
    }
}