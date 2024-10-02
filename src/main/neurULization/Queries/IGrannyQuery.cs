using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Queries
{
    public interface IGrannyQuery
    {
        bool RequiresPrecedingGrannyQueryResult { get; }

        Task<NeuronQuery> GetQuery(
            IEnsembleRepository ensembleRepository, 
            Ensemble ensemble, 
            IList<IGranny> retrievedGrannies, 
            string userId
        );
    }
}
