using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class InstantiatesClassParameterSet : IInstantiatesClassParameterSet
    {
        public InstantiatesClassParameterSet(
            Neuron @class
            )
        {
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));

            Class = @class;
        }

        public Neuron Class { get; }
    }
}
