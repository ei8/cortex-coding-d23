using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Selectors
{
    public class MatchExclude : ISelector
    {
        private readonly Guid[] neuronIds;

        public MatchExclude(params Guid[] neuronIds)
        {
            this.neuronIds = neuronIds;
        }

        public IEnumerable<Guid> Evaluate(Ensemble ensemble, IEnumerable<Guid> selection)
        {
            throw new NotImplementedException();
        }
    }
}
