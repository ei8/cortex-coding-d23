using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public class InstantiatesClassParameterSet : IInstantiatesClassParameterSet
    {
        public InstantiatesClassParameterSet(
            Neuron granny,
            Neuron @class
            )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));

            this.Granny = granny;
            this.Class = @class;
        }

        public Neuron Granny { get; }

        public Neuron Class { get; }
    }
}
