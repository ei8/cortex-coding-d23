using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Queries
{
    public class GrannyQuery : IGrannyQuery
    {
        private NeuronQuery neuronQuery;

        public GrannyQuery(NeuronQuery neuronQuery)
        {
            AssertionConcern.AssertArgumentNotNull(neuronQuery, nameof(neuronQuery));
            this.neuronQuery = neuronQuery;
        }

        public Task<NeuronQuery> GetQuery(ObtainParameters obtainParameters)
        {
            return Task.FromResult(neuronQuery);
        }
    }
}
