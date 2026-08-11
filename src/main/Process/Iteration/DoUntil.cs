using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Iteration
{
    public class DoUntil : IProcess
    {
        public enum WorkingMemoryKeys
        {
            Actions,
            Variable,
            Condition
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private IWorkingMemory<WorkingMemoryKeys>? workingMemory;
        private Action? completionCallback;

        public DoUntil()
        {
        }

        public void Initialize(IWorkingMemory workingMemory, Action completionCallback)
        {
            this.workingMemory = (IWorkingMemory<WorkingMemoryKeys>)workingMemory;
            this.completionCallback = completionCallback;
        }

        public IEnumerable<Neuron> GetCurrent()
        {
            var result = Enumerable.Empty<Neuron>();
            if (
                this.workingMemory != null &&
                this.workingMemory.TryGetContents<
                    WorkingMemoryKeys,
                    ReadableKeyedChunk<WorkingMemoryKeys>,
                    IEnumerable<Neuron>
                >(
                    WorkingMemoryKeys.Actions,
                    out var actions
                ) &&
                this.workingMemory.TryGetContents<
                    WorkingMemoryKeys,
                    WriteableKeyedChunk<WorkingMemoryKeys>,
                    IList<Neuron>
                >(
                    WorkingMemoryKeys.Variable,
                    out var variable
                )
            )
                result = [
                    ..actions,
                    ..variable
                ];

            return result;
        }

        public void HandleFire(Neuron target, ReadOnlyNetwork network)
        {
            DoUntil.logger.Info(
                new LogMessageGenerator(
                    () => $"Fired: {target.ToReadableString()}"
                )
            );

            if (this.workingMemory != null)
            {
                var presynaptics = network.GetPresynapticNeurons(target.Id).ToArray();
                if (
                    presynaptics.Any() &&
                    this.workingMemory.TryGetContents<
                        WorkingMemoryKeys,
                        WriteableKeyedChunk<WorkingMemoryKeys>,
                        IList<Neuron>
                    >(
                        WorkingMemoryKeys.Variable,
                        out var variables
                    ) &&
                    presynaptics.Intersect(variables).Any()
                )
                {
                    variables.Clear();
                    variables.Add(target);
                    DoUntil.logger.Info(
                        new LogMessageGenerator(
                            () => $"Updated variable to: {target.ToReadableString()}"
                        )
                    );
                }

                if (
                    this.workingMemory.TryGetContents<
                        WorkingMemoryKeys,
                        ReadableKeyedChunk<WorkingMemoryKeys>,
                        IEnumerable<Neuron>
                    >(
                        WorkingMemoryKeys.Condition,
                        out var condition
                    ) &&
                    target.Id == condition.Single().Id &&
                    this.completionCallback != null
                )
                    this.completionCallback();
            }
        }
    }
}