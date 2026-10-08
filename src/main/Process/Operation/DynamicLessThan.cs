using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public class DynamicLessThan
    (
        Comparator.WorkingMemoryInfo workingMemory,
        Action<DynamicLessThan, IEnumerable<Neuron>> completionCallback
    ) :
        FiniteProcessBase
        <
            Comparator.WorkingMemoryInfo,
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

            int? digitIndex = 
                ChunkHelper.GetOperandCurrentIndex
                (
                    this.WorkingMemory.CurrentComparand1Digit,
                    this.WorkingMemory.Comparand1,
                    this.WorkingMemory.CurrentComparand2Digit,
                    this.WorkingMemory.Comparand2
                );

            if (digitIndex.HasValue)
                this.WorkingMemory.AddCurrent(result, digitIndex.Value);
            else
                this.Complete();

            return result;
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            // TODO: Refactor with DynamicAddition
            if
            (
                (
                    this.WorkingMemory.PreviousComparand1Digit.HasCurrentChanged(this.WorkingMemory.CurrentComparand1Digit) ||
                    this.WorkingMemory.PreviousComparand2Digit.HasCurrentChanged(this.WorkingMemory.CurrentComparand2Digit)
                ) &&
                (
                    !ChunkHelper.HandleFire
                    (
                        targetNeuron,
                        this.WorkingMemory.CurrentComparand1Digit,
                        this.WorkingMemory.Comparand1,
                        this.WorkingMemory.CurrentComparand2Digit,
                        this.WorkingMemory.Comparand2,
                        [
                            new
                            (
                                this.WorkingMemory.AreEqualValues,
                                this.WorkingMemory.AreEqual
                            ),
                            new
                            (
                                this.WorkingMemory.IsLessThanValues,
                                this.WorkingMemory.IsLessThan
                            ),
                            new
                            (
                                this.WorkingMemory.IsGreaterThanValues,
                                this.WorkingMemory.IsGreaterThan
                            )
                        ],
                        out var allResultsHandled
                    )
                    ||
                    allResultsHandled
                )
            )
            {
                this.WorkingMemory.PreviousComparand1Digit = this.WorkingMemory.PreviousComparand1Digit.GetIfUnequal(this.WorkingMemory.CurrentComparand1Digit);
                this.WorkingMemory.PreviousComparand2Digit = this.WorkingMemory.PreviousComparand2Digit.GetIfUnequal(this.WorkingMemory.CurrentComparand2Digit);
                this.WorkingMemory.CurrentComparand1Digit = this.WorkingMemory.Comparand1.Content.GetAdjacentOrDefault(this.WorkingMemory.CurrentComparand1Digit, false);
                this.WorkingMemory.CurrentComparand2Digit = this.WorkingMemory.Comparand2.Content.GetAdjacentOrDefault(this.WorkingMemory.CurrentComparand2Digit, false);

                if
                (
                    (
                        this.WorkingMemory.CurrentComparand1Digit == null &&
                        this.WorkingMemory.CurrentComparand2Digit == null
                    ) ||
                    this.WorkingMemory.IsLessThan.Content.Last().Value.Tag.EndsWith("1") || 
                    this.WorkingMemory.IsGreaterThan.Content.Last().Value.Tag.EndsWith("1")
                )
                    this.Complete();
            }
        }

        private void Complete()
        {
            this.completionCallback(this, this.WorkingMemory.IsLessThan.Content.Select(c => c.Value));
            this.WorkingMemory.AreEqual.Content.Clear();
            this.WorkingMemory.IsLessThan.Content.Clear();
            this.WorkingMemory.IsGreaterThan.Content.Clear();
        }
    }
}
