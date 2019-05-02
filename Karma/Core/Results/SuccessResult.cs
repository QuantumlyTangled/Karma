using Finite.Commands;

namespace Karma.Core.Results
{
    internal struct SuccessResult : IResult
    {
        public static readonly SuccessResult Instance
            = new SuccessResult();

        public bool IsSuccess
            => true;
    }

}