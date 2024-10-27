using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface IGreatGrannyProcess<TResult>
        where TResult : IGranny
    {
        IGranny Execute(
            IGreatGrannyInfo<TResult> greatGrannyInfo,
            Ensemble ensemble,
            IGranny precedingGranny,
            TResult tempResult
            );
    }
}
