using neurUL.Common.Domain.Model;
using System;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstantiatesClassParameterSet : IInstantiatesClassParameterSet
    {
        public InstantiatesClassParameterSet(
            Neuron @class
            )
        {
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));

            this.Class = @class;
        }

        public Neuron Class { get; }
    }
}
