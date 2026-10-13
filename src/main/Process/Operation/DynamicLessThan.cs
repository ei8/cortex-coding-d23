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
        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            int? digitIndex = 
                ChunkHelper.GetOperandCurrentIndex
                (
                    this.WorkingMemory.Comparand1,
                    this.WorkingMemory.Comparand2
                );

            if (digitIndex.HasValue)
            {
                result.AddRange(this.WorkingMemory.GetCurrent(digitIndex.Value).Select(c => c.Neuron));
            }
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
                    this.WorkingMemory.Comparand1.HasCurrentDigitChanged() ||
                    this.WorkingMemory.Comparand2.HasCurrentDigitChanged()
                ) &&
                (
                    !ChunkHelper.HandleFire
                    (
                        targetNeuron,
                        this.WorkingMemory.Comparand1,
                        this.WorkingMemory.Comparand2,
                        false,
                        [
                            this.WorkingMemory.AreEqual,
                            this.WorkingMemory.IsLessThan,
                            this.WorkingMemory.IsGreaterThan
                        ],
                        out var allResultsHandled
                    ) ||
                    allResultsHandled.GetValueOrDefault(false)
                )
            )
            { 
                this.WorkingMemory.Comparand1.UpdateTracking(false);
                this.WorkingMemory.Comparand2.UpdateTracking(false);
                if
                (
                    (
                        this.WorkingMemory.Comparand1.CurrentDigit == null &&
                        this.WorkingMemory.Comparand2.CurrentDigit == null
                    ) ||
                    this.WorkingMemory.IsLessThan.Value.Items.Last().Neuron.Tag.EndsWith('1') || 
                    this.WorkingMemory.IsGreaterThan.Value.Items.Last().Neuron.Tag.EndsWith('1')
                )
                    this.Complete();
            }
        }

        private void Complete()
        {
            this.completionCallback(this, this.WorkingMemory.IsLessThan.Value.Items.Select(c => c.Neuron));
            this.WorkingMemory.AreEqual.Value.Items.Clear();
            this.WorkingMemory.IsLessThan.Value.Items.Clear();
            this.WorkingMemory.IsGreaterThan.Value.Items.Clear();
        }
    }
}
