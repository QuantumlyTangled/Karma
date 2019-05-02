using System;
using System.Threading.Tasks;
using Discord;
using Finite.Commands;
using Karma.Core.Results;
using Karma.Core.Configuration;

namespace Karma.Core.Pipelines
{
    public class PrefixPipeline : IPipeline
    {
        private readonly MasterConfig _config;
        
        public PrefixPipeline(MasterConfig config) => _config = config;
        
        public async Task<IResult> ExecuteAsync(CommandExecutionContext context, Func<Task<IResult>> next)
        {
            var ctx = context.Context as SystemContext;
            var msg = (IUserMessage) ctx.Message;
            var content = msg.Content;
            var userMention = $"{ctx.Client.CurrentUser.Mention} ";
            var guildMention = userMention.Replace("!", "");

            if (msg.Author.IsBot || msg.Author.IsWebhook)
                return new PrefixResult();

            if (content.StartsWith(_config.DiscordConfig.Prefix, StringComparison.CurrentCultureIgnoreCase))
                return await ValidPrefixExecuteAsync(context, _config.DiscordConfig.Prefix.Length, msg, next);
            if (content.StartsWith(userMention, StringComparison.CurrentCultureIgnoreCase))
                return await ValidPrefixExecuteAsync(context, userMention.Length, msg, next);
            if (content.StartsWith(guildMention, StringComparison.CurrentCultureIgnoreCase))
                return await ValidPrefixExecuteAsync(context, guildMention.Length, msg, next);
            return new PrefixResult();
        }
        
        private async Task<IResult> ValidPrefixExecuteAsync(CommandExecutionContext context, int prefixLength, IUserMessage msg, Func<Task<IResult>> next)
        {
            if (msg.Content.Length <= prefixLength) return new PrefixResult();
            context.PrefixLength = prefixLength;
            return await next();
        }
    }
}