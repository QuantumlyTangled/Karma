using System;
using System.Reflection;
using Discord.WebSocket;
using Finite.Commands;
using Finite.Commands.Extensions;
using Microsoft.Extensions.Configuration;
using Karma.Core;
using Karma.Core.Factories;
using Karma.Core.TypeReaders;
using Karma.Core.Pipelines;
using Karma.Core.Parsers;
using Karma.Events;
using Karma.Events.OnMessageEvents;
using Microsoft.Extensions.DependencyInjection;

namespace Karma
{
    public class Manager
    {
        private readonly IConfiguration _config;
        private readonly DiscordShardedClient _client;
        
        private CommandService<SystemContext> _commandService;
        private EventLoader _eventLoader;
        private IServiceProvider _serviceProvider;
        
        public Manager(IConfiguration config, DiscordShardedClient client)
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
                .AddPipeline<FinalizePipeline>()
                .BuildCommandService();

            _serviceProvider = new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton(_client)
                .AddSingleton(_commandService)
                .BuildServiceProvider();
            
            _eventLoader = new EventLoader()
                .LoadEvent(new OnMessageReceivedEvent(_client)
                    .AddSubEvent(new OnCommandSubEvent(_client, _commandService, _serviceProvider))
                );
        }
    }
}