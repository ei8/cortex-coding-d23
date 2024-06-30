using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Queries
{
    public class GrannyQueryBuilder : IReceiver
    {
        private Func<Neuron, NeuronQuery> queryWithNeuronBuilder;
        private Neuron retrievalResult;

        public GrannyQueryBuilder(Func<Neuron, NeuronQuery> queryWithNeuronBuilder)
        {
            AssertionConcern.AssertArgumentNotNull(queryWithNeuronBuilder, nameof(queryWithNeuronBuilder));
            this.queryWithNeuronBuilder = queryWithNeuronBuilder;
            this.retrievalResult = null;
        }

        public Task<NeuronQuery> GetQuery(ObtainParameters obtainParameters)
        {
            AssertionConcern.AssertStateTrue(this.retrievalResult != null, "RetrievalResult is required to invoke GetQuery.");
            return Task.FromResult(queryWithNeuronBuilder(this.retrievalResult));
        }

        public void SetPrecedingRetrievalResult(Neuron value)
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            this.retrievalResult = value;
        }
    }
}
