using System;
using System.Threading.Tasks;
using Finite.Commands;
using Karma.Core.Results;

namespace Karma.Core.Attributes
{
    public interface IPreconditionAttribute
    {
        Task<PreconditionResult> CheckPermissionsAsync(SystemContext context, CommandInfo command, IServiceProvider services);
    }
}