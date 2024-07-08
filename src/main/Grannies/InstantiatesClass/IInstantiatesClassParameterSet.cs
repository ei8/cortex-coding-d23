using System;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstantiatesClassParameterSet : IParameterSet
    {
        Neuron Class { get; }
    }
}
