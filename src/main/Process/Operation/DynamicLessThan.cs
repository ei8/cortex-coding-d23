using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public class DynamicLessThan
    (
        LessThan.WorkingMemoryInfo workingMemory,
        Action<DynamicLessThan, IEnumerable<Neuron>> completionCallback
    ) :
        FiniteProcessBase
        <
            LessThan.WorkingMemoryInfo,
            Action<DynamicLessThan, IEnumerable<Neuron>>
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

            int? currentComparand1DigitIndex = this.WorkingMemory.CurrentComparand1Digit != null ?
                this.WorkingMemory.Comparand1.Content.ToList().IndexOf(this.WorkingMemory.CurrentComparand1Digit) :
                null;

            int? currentComparand2DigitIndex = this.WorkingMemory.CurrentComparand2Digit != null ?
                this.WorkingMemory.Comparand2.Content.ToList().IndexOf(this.WorkingMemory.CurrentComparand2Digit) :
                null;

            int? digitIndex = currentComparand1DigitIndex.HasValue && currentComparand2DigitIndex.HasValue ?
                System.Math.Max(currentComparand1DigitIndex.Value, currentComparand2DigitIndex.Value) :
                currentComparand1DigitIndex ?? currentComparand2DigitIndex;

            if (digitIndex.HasValue)
                this.WorkingMemory.AddCurrent(result, digitIndex.Value);
            else
                this.Complete();

            return result;
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            // TODO: Refactor with DynamicAddition
            // if one of specified result values, add to results
            if
            (
                this.WorkingMemory.ResultValues.Content.Any(c => c.Value == targetNeuron) &&
                (
                    this.WorkingMemory.PreviousComparand1Digit.HasCurrentChanged(this.WorkingMemory.CurrentComparand1Digit) ||
                    this.WorkingMemory.PreviousComparand2Digit.HasCurrentChanged(this.WorkingMemory.CurrentComparand2Digit)
                )
            )
            {
                this.WorkingMemory.PreviousComparand1Digit = this.WorkingMemory.PreviousComparand1Digit.GetIfUnequal(this.WorkingMemory.CurrentComparand1Digit);
                this.WorkingMemory.PreviousComparand2Digit = this.WorkingMemory.PreviousComparand2Digit.GetIfUnequal(this.WorkingMemory.CurrentComparand2Digit);

                this.WorkingMemory.Result.Content.Add(new(targetNeuron));

                DynamicLessThan.logger.Info(new LogMessageGenerator(() => $"Added to Result(s): {targetNeuron.Tag}"));

                this.WorkingMemory.CurrentComparand1Digit = this.WorkingMemory.Comparand1.Content.GetAdjacentOrDefault(this.WorkingMemory.CurrentComparand1Digit, false);
                this.WorkingMemory.CurrentComparand2Digit = this.WorkingMemory.Comparand2.Content.GetAdjacentOrDefault(this.WorkingMemory.CurrentComparand2Digit, false);

                if
                (
                    (
                        this.WorkingMemory.CurrentComparand1Digit == null &&
                        this.WorkingMemory.CurrentComparand2Digit == null
                    ) ||
                    // TODO: CanShortCircuit predicate
                    targetNeuron.Tag.EndsWith("1")
                )
                    this.Complete();
            }
        }

        private void Complete()
        {
            this.completionCallback(this, this.WorkingMemory.Result.Content.Select(c => c.Value));
            this.WorkingMemory.Result.Content.Clear();
        }
    }
}
