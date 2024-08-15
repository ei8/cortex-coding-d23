using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    internal interface IGreatGrannyReadProcess<TResult>
    {
        IGranny Execute(
            IGreatGrannyInfo<TResult> greatGrannyProcess,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TResult tempResult
            );
    }
}
