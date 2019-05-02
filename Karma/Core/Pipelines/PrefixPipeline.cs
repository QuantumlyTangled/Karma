using System;
using System.Threading.Tasks;
using Discord;
using Finite.Commands;
using Karma.Core.Results;
using Microsoft.Extensions.Configuration;

namespace Karma.Core.Pipelines
{
    public class PrefixPipeline : IPipeline
    {
        private readonly IConfiguration _config;
        
        public PrefixPipeline(IConfiguration config) => _config = config;
        
        public async Task<IResult> ExecuteAsync(CommandExecutionContext context, Func<Task<IResult>> next)
        {
            var ctx = context.Context as SystemContext;
            var msg = (IUserMessage) ctx.Message;
            var content = msg.Content;

            if (msg.Author.IsBot || msg.Author.IsWebhook)
                return new PrefixResult();

            if (content.StartsWith(_config["prefix"], StringComparison.CurrentCultureIgnoreCase))
                return await ValidPrefixExecuteAsync(context, _config["prefix"].Length, msg, next);
            if (content.StartsWith(ctx.Client.CurrentUser.Mention, StringComparison.CurrentCultureIgnoreCase))
                return await ValidPrefixExecuteAsync(context, ctx.Client.CurrentUser.Mention.Length, msg, next);
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