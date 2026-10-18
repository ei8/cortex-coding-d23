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
                this.WorkingMemory.Action.Neuron,
                this.WorkingMemory.CounterVariable.Neuron
            ];

        public override bool HandleFire(Neuron targetNeuron)
        {
            bool result = false;
            if (this.WorkingMemory != null)
            {
                INeuronChunk? targetChunk = null;
                if 
                (
                    (targetChunk = this.WorkingMemory.CounterVariableValues.Items.SingleOrDefault(nc => nc.Neuron == targetNeuron)) != null &&
                    this.WorkingMemory.CounterVariable != targetChunk
                )
                {
                    this.WorkingMemory.CounterVariable = targetChunk;
                    this.counterChangedCallback?.Invoke(this);

                    DoUntil.logger.Info(
                        new LogMessageGenerator(
                            () => $"Updated variable to: {targetNeuron.ToReadableString()}"
                        )
                    );

                    result = true;
                }

                if (targetNeuron == this.WorkingMemory.Condition.Neuron)
                    this.completionCallback(this);
            }
            return result;
        }
    }
}