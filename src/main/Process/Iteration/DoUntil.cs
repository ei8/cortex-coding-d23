using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Iteration
{
    public partial class DoUntil
    (
        DoUntil.WorkingMemoryInfo workingMemory,
        Action<DoUntil>? counterChangedCallback,
        Action<DoUntil> completionCallback
    ) :
        FiniteProcessBase
        <
            DoUntil, 
            DoUntil.WorkingMemoryInfo,
            Action<DoUntil>
        >
        (
            workingMemory, 
            completionCallback
        )
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected Action<DoUntil>? counterChangedCallback = counterChangedCallback;

        public override IEnumerable<Neuron> GetCurrent() => 
            [
                this.WorkingMemory.Action.Value,
                this.WorkingMemory.CounterVariable.Value
            ];

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            if (this.WorkingMemory != null)
            {
                NeuronChunk? targetChunk = null;
                if 
                (
                    (targetChunk = this.WorkingMemory.CounterVariableValues.Content.SingleOrDefault(nc => nc.Value == targetNeuron)) != null &&
                    this.WorkingMemory.CounterVariable != targetChunk
                )
                {
                    this.WorkingMemory.CounterVariable = targetChunk;
                    if (this.counterChangedCallback != null)
                        this.counterChangedCallback(this);

                    DoUntil.logger.Info(
                        new LogMessageGenerator(
                            () => $"Updated variable to: {targetNeuron.ToReadableString()}"
                        )
                    );
                }

                if (targetNeuron == this.WorkingMemory.Condition.Content)
                    this.completionCallback(this);
            }
        }
    }
}