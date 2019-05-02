using System.Collections.Generic;
using System.Text;
using Karma.Core.Enums;
using Karma.Core.Results;

namespace Karma.Core
{
    public class Analytics
    {
        private readonly BotLog _botLog;

        public uint ReceivedMessages { get; set; } = 0;
        public uint UpdatedMessages { get; set; } = 0;
        public uint DeletedMessages { get; set; } = 0;
        public uint FilterDeletedMessages { get; set; } = 0;

        public Dictionary<string, int> UsedCommands { get; } = new Dictionary<string, int>();

        public Analytics(BotLog botLog) => _botLog = botLog;

        public void ExecutedCommand(SystemContext ctx, CommandResult result) {
            var cmdString = result.CommandPath;
            var guild = ctx.Guild;

            if (UsedCommands.ContainsKey(cmdString)) UsedCommands[cmdString]++;
            else UsedCommands.Add(cmdString, 1);

            var output = new StringBuilder()
                .AppendFormat($"<{guild.Name} <{guild.Id.ToString()}>>").AppendLine()
                .AppendFormat($"- Command : {cmdString}").AppendLine();
            
            if (ctx.Message.Content.Length < 250)
                output.AppendFormat($"- Content : {ctx.Message.Content}").AppendLine();

            output.AppendLine("+ Result  : Completed");

            _botLog.SendBotLogAsync(BotLogType.CommandManager, output.ToString()).GetAwaiter();
        }
    }
}