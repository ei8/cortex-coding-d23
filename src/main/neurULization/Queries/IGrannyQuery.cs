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
            INetworkRepository networkRepository, 
            Network network, 
            IList<IGranny> retrievedGrannies
        );
    }
}
