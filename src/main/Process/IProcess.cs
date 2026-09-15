using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    // TODO: Should this be a granny of its sub-circuits
    public interface IProcess
    {
        IEnumerable<Neuron> GetCurrent();

        void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network);
    }

    public interface IProcess<T> : IProcess
    {
        T WorkingMemory { get; }
    }
}
