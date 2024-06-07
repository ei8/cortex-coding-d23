using System;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IDependencyParameterSet : IParameterSet
    {
        Neuron Value { get; }

        Neuron Type { get; }
    }
}
