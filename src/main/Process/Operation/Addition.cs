using ei8.Cortex.Coding.d23.Process.Iteration;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class Addition :
        FiniteCompositeProcessBase
        <
            Addition, 
            Addition.WorkingMemoryInfo, 
            DoUntil,
            IEnumerable<Neuron>
        >
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Func<Neuron, int> digitRetriever;
        private readonly Func<int, Addition.WorkingMemoryInfo, IEnumerable<Neuron>> addendsRetriever;

        private Neuron? lastSumDigit;

        [SetsRequiredMembers]
        public Addition
        (
            WorkingMemoryInfo workingMemory,
            ReadOnlyNeuronChunk action,
            EnumerableChunk digitVariableValues,
            WriteableNeuronChunk digitVariable,
            Func<Neuron, int> digitRetriever,
            Func<int, Addition.WorkingMemoryInfo, IEnumerable<Neuron>> addendsRetriever,
            Action<Addition, IProcess?, IEnumerable<Neuron>> completionCallback
        ) :
            base
            (
                workingMemory,
                completionCallback
            )
        {
            this.Process1 = new
            (
                new
                (
                    action,
                    digitVariableValues,
                    digitVariable,
                    new(digitVariableValues.Content.Last())
                ),
                null,
                this.Complete
            );

            this.digitRetriever = digitRetriever;
            this.addendsRetriever = addendsRetriever;
        }

        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            result.AddRange(this.DoUntil.GetCurrent());

            var digitIndex = this.digitRetriever(this.DoUntil.WorkingMemory.CounterVariable.Value);
            if (digitIndex == 0)
                result.Add(this.WorkingMemory.PrecedingCarryOverValues.Content.Single(n => n.Tag.EndsWith('0')));
            else if (this.WorkingMemory.CarryOver.Content != null)
                result.Add(this.WorkingMemory.CarryOver.Content);

            var addends = this.addendsRetriever(digitIndex, this.WorkingMemory);
            if (!addends.Any())
                this.Complete(null);
            else
                result.AddRange(addends);

            return result;
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            this.Process1.HandleFire(targetNeuron, network);

            // if one of specified sum values, add to sums
            if
            (
                this.WorkingMemory.SumValues.Content.Contains(targetNeuron) &&
                lastSumDigit != this.Process1.WorkingMemory.CounterVariable.Value
            )
            {
                Addition.logger.Info(new LogMessageGenerator(() => $"Added to Sum(s): {targetNeuron.Tag}"));

                this.lastSumDigit = this.Process1.WorkingMemory.CounterVariable.Value;
                this.WorkingMemory.Sums.Content.Add(targetNeuron);
            }

            // if one of specified carry over values, update carry over
            if(this.WorkingMemory.CarryOverValues.Content.Contains(targetNeuron))
            {
                this.WorkingMemory.CarryOver.Content = 
                    this.WorkingMemory.PrecedingCarryOverValues.Content.Single
                    (
                        n => n.Tag.EndsWith(targetNeuron.Tag.Last())
                    );
            }
        }

        private void Complete(IProcess? process)
        {
            this.completionCallback(this, process, this.WorkingMemory.Sums.Content);
            this.WorkingMemory.Sums.Content.Clear();
        }

        public DoUntil DoUntil => this.Process1;
    }
}
