using System;
using System.Threading.Tasks;
using Discord;
using Finite.Commands;
using Microsoft.Extensions.DependencyInjection;
using Karma.Core.Results;
using Karma.Core.Configuration;

namespace Karma.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class BotPerms : Attribute, IPreconditionAttribute
    {
        private readonly GuildPermission? _guildPermission;
        private readonly ChannelPermission? _channelPermission;

        public BotPerms(GuildPermission guildPermission) => _guildPermission = guildPermission;

        public BotPerms(ChannelPermission channelPermission) => _channelPermission = channelPermission;

        public Task<PreconditionResult> CheckPermissionsAsync(SystemContext context, CommandInfo command, IServiceProvider services)
        {
            var config = services.GetService<MasterConfig>();
            var self = context.Guild.GetCurrentUserAsync().GetAwaiter().GetResult();

            if (_guildPermission.HasValue)
            {
                if (!(self.GuildPermissions.Has(_guildPermission.Value) || self.GuildPermissions.Administrator))
                    return Task.FromResult(PreconditionResult.FromError($"I do not have sufficient permissions to perform the requested command! {Format.Bold($"SERVER-PERM-MISSING : {_guildPermission.ToString()}")}"));
            }
            else if (_channelPermission.HasValue)
            {
                var channelPerms = self.GetPermissions((IGuildChannel)context.Channel);
                if (!(channelPerms.Has(_channelPermission.Value) || self.GuildPermissions.Administrator))
                    return Task.FromResult(PreconditionResult.FromError($"I do not have sufficient permissions to perform the requested command! {Format.Bold($"CHANNEL-PERM-MISSING : {_channelPermission.ToString()}")}"));
            }

            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}