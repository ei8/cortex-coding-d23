using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal interface IInnerProcessTargetAsync<TResult>
    {
        Task<IGranny> ExecuteAsync(
            IInnerProcess<TResult> innerProcess,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            IGranny precedingGranny,
            TResult tempResult
            );
    }

    internal interface IInnerProcessTarget<TResult>
    {
        IGranny Execute(
            IInnerProcess<TResult> innerProcess,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            IGranny precedingGranny,
            TResult tempResult
            );
    }
}
