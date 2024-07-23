using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal interface IGreatGrannyProcessAsync<TResult>
    {
        Task<IGranny> ExecuteAsync(
            IGreatGrannyInfo<TResult> greatGrannyProcess,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            IGranny precedingGranny,
            TResult tempResult
            );
    }

    internal interface IGreatGrannyProcess<TResult>
    {
        IGranny Execute(
            IGreatGrannyInfo<TResult> greatGrannyProcess,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            IGranny precedingGranny,
            TResult tempResult
            );
    }
}
