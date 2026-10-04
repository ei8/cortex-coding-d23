using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public class DynamicAddition
    (
        Addition.WorkingMemoryInfo workingMemory,
        Action<DynamicAddition, IEnumerable<Neuron>> completionCallback
    ) :
        FiniteProcessBase
        <
            Addition.WorkingMemoryInfo,
            Action<DynamicAddition, IEnumerable<Neuron>>
        >
        (
            workingMemory,
            completionCallback
        )
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            // TODO: Refactor with DynamicLessThan
            int? currentAugendDigitIndex = this.WorkingMemory.CurrentAugendDigit != null ?
                this.WorkingMemory.Augend.Content.ToList().IndexOf(this.WorkingMemory.CurrentAugendDigit) :
                null;

            int? currentAddendDigitIndex = this.WorkingMemory.CurrentAddendDigit != null ?
                this.WorkingMemory.Addend.Content.ToList().IndexOf(this.WorkingMemory.CurrentAddendDigit) :
                null;

            int? digitIndex = currentAugendDigitIndex.HasValue && currentAddendDigitIndex.HasValue ?
                System.Math.Max(currentAugendDigitIndex.Value, currentAddendDigitIndex.Value) :
                currentAugendDigitIndex ?? currentAddendDigitIndex;

            if (digitIndex.HasValue)
                this.WorkingMemory.AddCurrent(result, digitIndex.Value);
            else
                this.Complete();

            return result;
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            // if one of specified sum values, add to sums
            if
            (
                this.WorkingMemory.SumValues.Content.Any(c => c.Value == targetNeuron) &&
                (
                    this.WorkingMemory.PreviousAugendDigit.HasCurrentChanged(this.WorkingMemory.CurrentAugendDigit) ||
                    this.WorkingMemory.PreviousAddendDigit.HasCurrentChanged(this.WorkingMemory.CurrentAddendDigit)
                )
            )
            {
                this.WorkingMemory.PreviousAugendDigit = this.WorkingMemory.PreviousAugendDigit.GetIfUnequal(this.WorkingMemory.CurrentAugendDigit);
                this.WorkingMemory.PreviousAddendDigit = this.WorkingMemory.PreviousAddendDigit.GetIfUnequal(this.WorkingMemory.CurrentAddendDigit);

                this.WorkingMemory.Sum.Content.Add(new(targetNeuron));

                DynamicAddition.logger.Info(new LogMessageGenerator(() => $"Added to Sum(s): {targetNeuron.Tag}"));

                this.WorkingMemory.CurrentAugendDigit = this.WorkingMemory.Augend.Content.GetAdjacentOrDefault(this.WorkingMemory.CurrentAugendDigit);
                this.WorkingMemory.CurrentAddendDigit = this.WorkingMemory.Addend.Content.GetAdjacentOrDefault(this.WorkingMemory.CurrentAddendDigit);

                if (this.WorkingMemory.CurrentAugendDigit == null && this.WorkingMemory.CurrentAddendDigit == null)
                    this.Complete();
            }

            // if one of specified carry over values, update carry over
            if(this.WorkingMemory.CarryOverValues.Content.Any(c => c.Value == targetNeuron))
            {
                this.WorkingMemory.CarryOver = 
                    this.WorkingMemory.PrecedingCarryOverValues.Content.Single
                    (
                        n => n.Value.Tag.EndsWith(targetNeuron.Tag.Last())
                    );
            }
        }

        private void Complete()
        {
            this.completionCallback(this, [.. this.WorkingMemory.GetSumNeurons()]);
            this.WorkingMemory.Sum.Content.Clear();
        }
    }
}
