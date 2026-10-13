using ei8.Cortex.Coding.d23.Process.Iteration;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class SequentialAddition :
        FiniteCompositeProcessBase
        <
            Addition.WorkingMemoryInfo, 
            DoUntil,
            Action<SequentialAddition, IProcess?, IEnumerable<Neuron>>
        >
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Func<Neuron, int?> digitRetriever;
        private Neuron? lastSumDigit;

        public SequentialAddition
        (
            Addition.WorkingMemoryInfo workingMemory,
            DoUntil.WorkingMemoryInfo doUntilWorkingMemory,
            Func<Neuron, int?> digitRetriever,
            Action<SequentialAddition, IProcess?, IEnumerable<Neuron>> completionCallback
        ) :
            base
            (
                workingMemory,
                completionCallback
            )
        {
            this.Process1 = new
            (
                doUntilWorkingMemory,
                null,
                this.Complete
            );

            this.digitRetriever = digitRetriever;
        }

        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            if (this.DoUntil != null)
            {
                result.AddRange(this.DoUntil.GetCurrent());

                var digitIndex = this.digitRetriever(this.DoUntil.WorkingMemory.CounterVariable.Neuron);
                if (digitIndex.HasValue)
                    result.AddRange(this.WorkingMemory.GetCurrent(digitIndex.Value).Select(c => c.Neuron));
                else
                    this.Complete(null);
            }

            return result;
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            if (this.DoUntil != null)
            {
                this.DoUntil.HandleFire(targetNeuron, network);

                // if one of specified sum values, add to sums
                if
                (
                    this.WorkingMemory.Sum.DigitValues.Items.Any(c => c.Neuron == targetNeuron) &&
                    this.lastSumDigit != this.DoUntil.WorkingMemory.CounterVariable.Neuron
                )
                {
                    SequentialAddition.logger.Info(new LogMessageGenerator(() => $"Added to Sum(s): {targetNeuron.Tag}"));

                    this.lastSumDigit = this.DoUntil.WorkingMemory.CounterVariable.Neuron;
                    this.WorkingMemory.Sum.Value.Items.Add(new NeuronChunk(targetNeuron));
                }
            }

            // TODO: Check if possible to update to copy DynamicAddition
            // if one of specified carry over values, update carry over
            if(this.WorkingMemory.CarryOver.DigitValues.Items.Any(c => c.Neuron == targetNeuron))
                this.WorkingMemory.CarryOver.Value.Items.Add(new NeuronChunk(targetNeuron));
        }

        private void Complete(IProcess? process)
        {
            this.completionCallback(this, process, [.. this.WorkingMemory.GetSumNeurons()]);
            this.WorkingMemory.Sum.Value.Items.Clear();
        }

        public DoUntil? DoUntil => this.Process1;
    }
}
