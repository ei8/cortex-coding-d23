using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal interface IInnerProcess<TResult>
    {
        IGranny Execute(
            Ensemble ensemble, 
            Id23neurULizationOptions options,
            IGranny precedingGranny, 
            TResult tempResult
            );

        Task<IGranny> ExecuteAsync(
            Ensemble ensemble,
            Id23neurULizationOptions options,
            IGranny precedingGranny,
            TResult tempResult
            );
    }
}
