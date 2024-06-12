using System;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstantiatesClassParameterSet : IAggregateParameterSet
    {
        Neuron Class { get; }
    }
}
