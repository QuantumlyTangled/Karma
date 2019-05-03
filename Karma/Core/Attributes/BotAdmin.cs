using System;
using System.Threading.Tasks;
using Finite.Commands;
using Microsoft.Extensions.DependencyInjection;
using Karma.Core.Results;
using Karma.Core.Configuration;

namespace Karma.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BotAdmin : Attribute, IPreconditionAttribute
    {
        public Task<PreconditionResult> CheckPermissionsAsync(SystemContext context, CommandInfo command, IServiceProvider services)
        {
            var config = services.GetService<MasterConfig>();
            var user = context.Guild.GetUserAsync(context.Author.Id).GetAwaiter().GetResult();

            if (user.Id == config.DiscordConfig.MasterAdminId || config.DiscordConfig.OtherAdmins.Contains(user.Id))
                return Task.FromResult(PreconditionResult.FromSuccess());
            
            return Task.FromResult(PreconditionResult.FromError("People with talent often have the wrong impression that things will go as they think."));
        }
    }
}