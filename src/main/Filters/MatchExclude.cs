using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Filters
{
    public class MatchExclude : IFilter
    {
        private readonly Guid[] neuronIds;

        public MatchExclude(params Guid[] neuronIds)
        {
            this.neuronIds = neuronIds;
        }

        public IEnumerable<Neuron> Evaluate(Ensemble ensemble, IEnumerable<Neuron> paths)
        {
            throw new NotImplementedException();
        }
    }
}
