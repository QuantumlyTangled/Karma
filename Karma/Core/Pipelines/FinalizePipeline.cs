using System;
using System.Threading.Tasks;
using Finite.Commands;
using Karma.Core.Results;

namespace Karma.Core.Pipelines
{
    public class FinalizePipeline : IPipeline
    {
        public async Task<IResult> ExecuteAsync(CommandExecutionContext context, Func<Task<IResult>> next)
        {
            var cmd = context.Command;

            try 
            { 
                await next(); 
                return new CommandResult(true, cmd);
            } 
            catch (Exception e) { return new CommandResult(false, cmd, e); }
        }
    }
}