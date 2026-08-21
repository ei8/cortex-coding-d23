using NLog;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Iteration
{
    public partial class DoUntil(DoUntil.WorkingMemoryInfo workingMemory) :
        FiniteProcessBase<DoUntil.WorkingMemoryInfo>(workingMemory)
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override IEnumerable<Neuron> GetCurrent() => 
            [
                this.WorkingMemory.Action.Value,
                this.WorkingMemory.CounterVariable.Value
            ];

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            base.HandleFire(targetNeuron, network);

            if (this.Status == FiniteStatus.Idle)
                this.Start();

            if (this.WorkingMemory != null)
            {
                var presynaptics = network.GetPresynapticNeurons(targetNeuron.Id).ToArray();
                if (
                    presynaptics.Length > 0 &&
                    this.WorkingMemory.CounterVariable != null &&
                    presynaptics.Contains(this.WorkingMemory.CounterVariable.Value)
                )
                {
                    this.WorkingMemory.CounterVariable.Value = targetNeuron;
                    DoUntil.logger.Info(
                        new LogMessageGenerator(
                            () => $"Updated variable to: {targetNeuron.ToReadableString()}"
                        )
                    );
                }

                if (targetNeuron == this.WorkingMemory.Condition.Content)
                    this.Complete();
            }
        }
    }
}