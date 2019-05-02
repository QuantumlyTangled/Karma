using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Finite.Commands;
using Microsoft.Extensions.DependencyInjection;
using Karma.Core;
using Karma.Core.Results;
using Karma.Core.Enums;

namespace Karma.Events.OnMessageEvents
{
    public class OnCommandSubEvent : IOnMessageSubEvent
    {
        private readonly IDiscordClient _client;
        private readonly CommandService<SystemContext> _commands;
        private readonly Analytics _analytics;
        private readonly BotLog _botLog;
        private readonly IServiceProvider _services;

        public OnCommandSubEvent(IDiscordClient client, CommandService<SystemContext> commands, Analytics analytics, BotLog botLog, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _analytics = analytics;
            _botLog = botLog;
            _services = services;
        }

        public async Task ExecuteAsync(SocketMessage message)
        {
            try
            {
                var msg = message as IUserMessage;
                var tc = msg.Channel as ITextChannel;
                
                using (var scope = _services.CreateScope())
                {
					var context = new SystemContext(_client, message, scope.ServiceProvider);
					var result = await _commands.ExecuteAsync(context, scope.ServiceProvider);

					switch (result)
                    {
                        case PreconditionResult r:
                            if (r.Silent) return;
                            await tc.SendMessageAsync($"{Format.Bold("Command Error!")} {r.Error}");
                            break;
	                    case CommandResult c:
                            if (result.IsSuccess)
                            {
                                if (!(result is CommandResult cmdResult)) return;

                                _analytics.ExecutedCommand(context, cmdResult);
                            }
                            else await LogCommandErrorAsync(context, c);
							break;
					}
				}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        private async Task LogCommandErrorAsync(SystemContext context, CommandResult result)
        {
            var output = new StringBuilder()
                .AppendFormat($"<{context.Guild.Name} <{context.Guild.Id.ToString()}>>").AppendLine()
                .AppendFormat($"---- Command : {result.CommandPath}").AppendLine()
                .AppendFormat($"---- Content : {context.Message.Content}").AppendLine()
                .AppendLine("---- Result  : FAILURE").AppendLine()
                .AppendLine(result.Exception.ToString());

            await _botLog.SendBotLogAsync(BotLogType.CommandManager, output.ToString(), true);

            var tcOutput = new StringBuilder()
                .AppendFormat("{0} Something bad has happened inside of me! I've alerted my developer about the problem. I hope they'll get me all patched up soon!", Format.Bold("OH NO!")).AppendLine()
                .AppendLine()
                .AppendFormat("{0} {1}", Format.Bold("ERROR :"), result.Exception.Message);

            await context.Channel.SendMessageAsync(tcOutput.ToString());
        }
    }
}