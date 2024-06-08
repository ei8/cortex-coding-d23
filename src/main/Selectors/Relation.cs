using ei8.Cortex.Library.Common;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Selectors
{
    public class Relation : ISelector
    {
        private readonly RelativeType type;

        public Relation(RelativeType type)
        {
            this.type = type;
        }

        public IEnumerable<Neuron> Evaluate(Ensemble ensemble, IEnumerable<Neuron> selection)
        {
            throw new System.NotImplementedException();
        }
    }
}
