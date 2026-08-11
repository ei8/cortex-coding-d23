using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public interface IProcess
    {
        void Initialize(IWorkingMemory workingMemory, Action completionCallback);

        IEnumerable<Neuron> GetCurrent();

        void HandleFire(Neuron target, ReadOnlyNetwork network);
    }
}
