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

        public GrannyQuery(NeuronQuery neuronQuery)
        {
            AssertionConcern.AssertArgumentNotNull(neuronQuery, nameof(neuronQuery));
            this.neuronQuery = neuronQuery;
        }

        public Task<NeuronQuery> GetQuery(ProcessParameters processParameters, IList<IGranny> retrievedGrannies)
        {
            return Task.FromResult(neuronQuery);
        }
    }
}
