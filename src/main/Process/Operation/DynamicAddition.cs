using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class DynamicAddition :
        FiniteProcessBase
        <
            DynamicAddition, 
            Addition.WorkingMemoryInfo,
            Action<DynamicAddition, IEnumerable<Neuron>>
        >
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Func<int> digitRetriever;
        private readonly Action digitUpdater;
        private readonly Func<int, Addition.WorkingMemoryInfo, IEnumerable<Neuron>> addendsRetriever;

        private int? lastSumDigit;

        [SetsRequiredMembers]
        public DynamicAddition
        (
            Addition.WorkingMemoryInfo workingMemory,
            Func<int> digitRetriever,
            Action digitUpdater,
            Func<int, Addition.WorkingMemoryInfo, IEnumerable<Neuron>> addendsRetriever,
            Action<DynamicAddition, IEnumerable<Neuron>> completionCallback
        ) :
            base
            (
                workingMemory,
                completionCallback
            )
        {
            this.digitRetriever = digitRetriever;
            this.digitUpdater = digitUpdater;
            this.addendsRetriever = addendsRetriever;
        }

        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            var digitIndex = this.digitRetriever();
            if (digitIndex == 0)
                result.Add(this.WorkingMemory.PrecedingCarryOverValues.Content.Single(n => n.Tag.EndsWith('0')));
            else if (this.WorkingMemory.CarryOver.Content != null)
                result.Add(this.WorkingMemory.CarryOver.Content);

            var addends = this.addendsRetriever(digitIndex, this.WorkingMemory);
            if (!addends.Any())
                this.Complete();
            else
                result.AddRange(addends);

            return result;
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            var currentDigit = this.digitRetriever();
            // if one of specified sum values, add to sums
            if
            (
                this.WorkingMemory.SumValues.Content.Contains(targetNeuron) &&
                this.lastSumDigit != currentDigit
            )
            {
                DynamicAddition.logger.Info(new LogMessageGenerator(() => $"Added to Sum(s): {targetNeuron.Tag}"));

                this.lastSumDigit = currentDigit;
                this.WorkingMemory.Sum.Content.Add(targetNeuron);
                this.digitUpdater();
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

        private void Complete()
        {
            var result = SequentialAddition.GetSum
            (
                this.WorkingMemory.Sum.Content,
                this.WorkingMemory.CarryOver.Value
            );

            this.completionCallback(this, [..result]);
            this.WorkingMemory.Sum.Content.Clear();
        }
    }
}
