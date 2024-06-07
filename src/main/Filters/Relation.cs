using ei8.Cortex.Library.Common;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Filters
{
    public class Relation : IFilter
    {
        private readonly RelativeType type;

        public Relation(RelativeType type)
        {
            this.type = type;
        }

        public IEnumerable<Neuron> Evaluate(Ensemble ensemble, IEnumerable<Neuron> paths)
        {
            throw new System.NotImplementedException();
        }
    }
}
