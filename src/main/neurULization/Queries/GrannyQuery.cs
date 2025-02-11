using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Queries
{
    public class GrannyQuery : IGrannyQuery
    {
        private NeuronQuery neuronQuery;

        public GrannyQuery(NeuronQuery neuronQuery, bool requiresPrecedingGrannyQueryResult = true)
        {
            AssertionConcern.AssertArgumentNotNull(neuronQuery, nameof(neuronQuery));
            this.neuronQuery = neuronQuery;
            this.RequiresPrecedingGrannyQueryResult = requiresPrecedingGrannyQueryResult;
        }

        public bool RequiresPrecedingGrannyQueryResult { get; }

        public Task<NeuronQuery> GetQuery(
            INetworkRepository networkRepository, 
            Network network, 
            IList<IGranny> retrievedGrannies, 
            string userId
        )
        {
            return Task.FromResult(neuronQuery);
        }
    }
}
