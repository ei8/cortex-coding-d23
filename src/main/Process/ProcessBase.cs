using NLog;
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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public abstract IEnumerable<Neuron> GetCurrent();

        public virtual void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            ProcessBase<T>.logger.Info(
                new LogMessageGenerator(
                    () => $"Fired: {targetNeuron.ToReadableString()}"
                )
            );
        }

        public T WorkingMemory { get; } = workingMemory;
    }
}
