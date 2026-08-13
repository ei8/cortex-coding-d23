using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Iteration
{
    public class DoUntil : IProcess
    {
        public class WorkingMemory(
            ReadOnlyChunk actions,
            WriteableChunk counterVariable,
            ReadOnlyChunk condition
        ) : IWorkingMemory
        {
            public ReadOnlyChunk Actions { get; } = actions;
            public WriteableChunk CounterVariable { get; } = counterVariable;
            public ReadOnlyChunk Condition { get; } = condition;
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private WorkingMemory? workingMemory;
        private Action? completionCallback;

        public DoUntil()
        {
        }

        public void Initialize(IWorkingMemory workingMemory, Action completionCallback)
        {
            this.workingMemory = (WorkingMemory) workingMemory;
            this.completionCallback = completionCallback;
        }

        public IEnumerable<Neuron> GetCurrent()
        {
            var result = Enumerable.Empty<Neuron>();
            if (
                this.workingMemory != null &&
                this.workingMemory.Actions != null &&
                this.workingMemory.CounterVariable != null
            )
                result = [
                    ..this.workingMemory.Actions.Contents,
                    ..this.workingMemory.CounterVariable.Contents
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
                    this.workingMemory.CounterVariable != null &&
                    presynaptics.Intersect(this.workingMemory.CounterVariable.Contents).Any()
                )
                {
                    this.workingMemory.CounterVariable.Contents.Clear();
                    this.workingMemory.CounterVariable.Contents.Add(target);
                    DoUntil.logger.Info(
                        new LogMessageGenerator(
                            () => $"Updated variable to: {target.ToReadableString()}"
                        )
                    );
                }

                if (
                    this.workingMemory.Condition != null &&
                    target.Id == this.workingMemory.Condition.Contents.Single().Id &&
                    this.completionCallback != null
                )
                    this.completionCallback();
            }
        }
    }
}