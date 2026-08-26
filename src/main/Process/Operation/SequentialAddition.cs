using ei8.Cortex.Coding.d23.Process.Iteration;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public class SequentialAddition :
        FiniteCompositeProcessBase
        <
            SequentialAddition, 
            Addition.WorkingMemoryInfo, 
            DoUntil,
            Action<SequentialAddition, IProcess?, IEnumerable<Neuron>>
        >
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Func<Neuron, int> digitRetriever;
        private readonly Func<int, Addition.WorkingMemoryInfo, IEnumerable<Neuron>> addendsRetriever;

        private Neuron? lastSumDigit;

        [SetsRequiredMembers]
        public SequentialAddition
        (
            Addition.WorkingMemoryInfo workingMemory,
            DoUntil.WorkingMemoryInfo doUntilWorkingMemory,
            Func<Neuron, int> digitRetriever,
            Func<int, Addition.WorkingMemoryInfo, IEnumerable<Neuron>> addendsRetriever,
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
            this.DoUntil.HandleFire(targetNeuron, network);

            // if one of specified sum values, add to sums
            if
            (
                this.WorkingMemory.SumValues.Content.Contains(targetNeuron) &&
                lastSumDigit != this.DoUntil.WorkingMemory.CounterVariable.Value
            )
            {
                SequentialAddition.logger.Info(new LogMessageGenerator(() => $"Added to Sum(s): {targetNeuron.Tag}"));

                this.lastSumDigit = this.DoUntil.WorkingMemory.CounterVariable.Value;
                this.WorkingMemory.Sum.Content.Add(targetNeuron);
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
            List<Neuron> result = SequentialAddition.GetSum
            (
                this.WorkingMemory.Sum.Content, 
                this.WorkingMemory.CarryOver.Value
            );

            this.completionCallback(this, process, [.. result]);
            this.WorkingMemory.Sum.Content.Clear();
        }

        internal static List<Neuron> GetSum(IEnumerable<Neuron> sums, Neuron? carryOver)
        {
            var result = new List<Neuron>(sums);

            if (carryOver != null)
                result.Add(carryOver);

            return result;
        }

        public DoUntil DoUntil => this.Process1;
    }
}
