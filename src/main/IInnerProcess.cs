using ei8.Cortex.Coding.d23.Grannies;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal interface IInnerProcess<TResult>
    {
        IGranny Execute(
            Ensemble ensemble, 
            IPrimitiveSet primitives, 
            IGranny precedingGranny, 
            TResult tempResult, 
            IEnsembleRepository ensembleRepository,
            string userId
            );

        Task<IGranny> ExecuteAsync(
            Ensemble ensemble,
            IPrimitiveSet primitives,
            IGranny precedingGranny,
            TResult tempResult,
            IEnsembleRepository ensembleRepository,
            string userId
            );
    }
}
