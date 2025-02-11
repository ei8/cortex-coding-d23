using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.neurULization.Selectors
{
    public class MatchExclude : ISelector
    {
        private readonly Guid[] neuronIds;

        public MatchExclude(params Guid[] neuronIds)
        {
            this.neuronIds = neuronIds;
        }

        public IEnumerable<Guid> Evaluate(Network network, IEnumerable<Guid> selection)
        {
            throw new NotImplementedException();
        }
    }
}
