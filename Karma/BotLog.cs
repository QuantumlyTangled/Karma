using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Karma.Core;
using Karma.Core.Configuration;
using Karma.Core.Enums;
using Karma.Core.Extensions;

namespace Karma
{
    public class BotLog
    {
        private readonly MasterConfig _config;
        private readonly IDiscordClient _client;

        public BotLog(MasterConfig config, DiscordShardedClient client) {
            _config = config;
            _client = client;
        }

        public async Task SendBotLogAsync(BotLogType logType, string entry, bool pingMaster = false)
        {
            if (_config.DiscordConfig.MasterGuildId == 0) return;
            
            var guild = await _client.GetGuildAsync(_config.DiscordConfig.MasterGuildId);
            var output = new StringBuilder()
                .AppendFormat("[ {0} ] {1} {2} {3}", 
                    DateTime.Now.ToString("HH:mm:ss"),
                    SystemUtilities.GetSeparator,
                    logType != BotLogType.Common ? $"{logType} {SystemUtilities.GetSeparator}" : "",
                    entry);

            try
            {
                var tc = GetLogChannel(logType, guild);
                var pingMasterStr = pingMaster ? $"<@!{_config.DiscordConfig.MasterAdminId.ToString()}>": string.Empty;

                if (output.Length > 1950) 
                {
                    await tc.SendStringAsFileAsync($"{logType}.log", output.ToString(), $"Error! Refer to file {pingMasterStr}!", false);
                    return;
                }

                await tc.SendMessageAsync(Format.Code(output.ToString(), "diff") + pingMasterStr);
            }
            finally
            {
                SystemUtilities.WriteToLogFile(Directories.BotLog, output.ToString());
            }    
        }

        private ITextChannel GetLogChannel(BotLogType logType, IGuild guild)
        {
            ulong tcId = 0;

            switch (logType) {
                case BotLogType.CommandManager:
                    tcId = _config.DiscordConfig.BotLogChannels.CommandMngr;
                    break;
                case BotLogType.GuildManager:
                    tcId = _config.DiscordConfig.BotLogChannels.GuildMngr;
                    break;
            }

            if (tcId != 0)
            {
                var tc = guild.GetTextChannelAsync(tcId).GetAwaiter().GetResult();
                if (tc != null) return tc;
            }          
            if (_config.DiscordConfig.BotLogChannels.Common != 0) 
            {
                var tc = guild.GetTextChannelAsync(_config.DiscordConfig.BotLogChannels.Common).GetAwaiter().GetResult();
                if (tc != null) return tc;
            }

            return null;
        }
    }
}