using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Finite.Commands;
using Microsoft.Extensions.DependencyInjection;
using Karma.Core;
using Karma.Core.Results;

namespace Karma.Events.OnMessageEvents
{
    public class OnCommandSubEvent : IOnMessageSubEvent
    {
        private readonly IDiscordClient _client;
        private readonly CommandService<SystemContext> _commands;
        private readonly IServiceProvider _services;

        public OnCommandSubEvent(IDiscordClient client, CommandService<SystemContext> commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task ExecuteAsync(SocketMessage message)
        {
            try
            {
                using (var scope = _services.CreateScope())
                {
					var context = new SystemContext(_client, message, scope.ServiceProvider);
					var result = await _commands.ExecuteAsync(context, scope.ServiceProvider);

					switch (result)
                    {
	                    case CommandResult c:
							if (result.IsSuccess)
                            {
                                if (!(result is CommandResult cmdResult)) return;
								if (cmdResult.CommandPath == "root dc") return;
                            } 
                            else Console.WriteLine($"{context} {c}");
							break;
					}
				}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}