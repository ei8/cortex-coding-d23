using System;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstantiatesParameterSet : IRepositoryParameterSet
    {
        Neuron Class { get; }

        IDependency Dependency { get; }
    }
}
