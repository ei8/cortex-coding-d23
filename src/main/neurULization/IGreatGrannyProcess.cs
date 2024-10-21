using ei8.Cortex.Coding.d23.Grannies;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface IGreatGrannyProcessAsync<TResult>
        where TResult : IGranny
    {
        Task<IGranny> ExecuteAsync(
            IGreatGrannyInfo<TResult> greatGrannyProcess,
            Ensemble ensemble,
            IGranny precedingGranny,
            TResult tempResult
            );
    }

    public interface IGreatGrannyProcess<TResult>
        where TResult : IGranny
    {
        IGranny Execute(
            IGreatGrannyInfo<TResult> greatGrannyProcess,
            Ensemble ensemble,
            IGranny precedingGranny,
            TResult tempResult
            );
    }
}
