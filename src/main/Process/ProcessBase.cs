using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class ProcessBase<T>
    (
        T workingMemory
    ) :
        IProcess<T>
        where T : IWorkingMemory
    {
        public abstract IEnumerable<Neuron> GetCurrent();

        public abstract bool HandleFire(Neuron targetNeuron);

        public T WorkingMemory { get; } = workingMemory;
    }
}
