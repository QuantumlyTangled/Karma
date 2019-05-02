using System;
using System.Threading.Tasks;
using Discord;
using Finite.Commands;
using Karma.Core.Results;

namespace Karma.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class NSFW : Attribute, IPreconditionAttribute
    {
        public Task<PreconditionResult> CheckPermissionsAsync(SystemContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.Channel is ITextChannel text && text.IsNsfw)
                return Task.FromResult(PreconditionResult.FromSuccess());
            return Task.FromResult(PreconditionResult.FromSilentError("Used Channel is not marked as NSFW"));
        }
    }
}