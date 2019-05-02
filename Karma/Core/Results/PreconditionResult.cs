using Finite.Commands;

namespace Karma.Core.Results
{
    public class PreconditionResult : IResult
    {
        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }
        public bool Silent { get; private set;  }

        public static PreconditionResult FromSuccess() 
            => new PreconditionResult() { IsSuccess = true };
        
        public static PreconditionResult FromError(string error)
            => new PreconditionResult() { IsSuccess = false, Error = error };
        
        public static PreconditionResult FromSilentSuccess() 
            => new PreconditionResult() { IsSuccess = true, Silent = true};
        
        public static PreconditionResult FromSilentError(string error)
            => new PreconditionResult() { IsSuccess = false, Error = error, Silent = true};
    }
}