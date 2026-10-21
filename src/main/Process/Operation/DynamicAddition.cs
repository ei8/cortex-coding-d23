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
        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            // TODO: Refactor with DynamicLessThan
            int? digitIndex =
                ChunkHelper.GetOperandCurrentIndex
                (
                    this.WorkingMemory.Augend,
                    this.WorkingMemory.Addend
                );

            if (digitIndex.HasValue)
                result.AddRange(this.WorkingMemory.GetCurrent(digitIndex.Value).Select(c => c.Neuron));
            else
                this.Complete();

            return result;
        }

        public override bool HandleFire(Neuron targetNeuron)
        {
            bool targetHandled = false;
            // if one of specified sum values, add to sums
            if
            (
                (
                    this.WorkingMemory.Augend.HasCurrentDigitChanged() ||
                    this.WorkingMemory.Addend.HasCurrentDigitChanged()
                ) &&
                this.WorkingMemory.TryHandleAllResults
                (
                    targetNeuron,
                    this.WorkingMemory.Augend,
                    this.WorkingMemory.Addend,
                    true,
                    out targetHandled
                )
            )
            {
                this.WorkingMemory.Augend.UpdateTracking();
                this.WorkingMemory.Addend.UpdateTracking();

                if 
                (
                    this.WorkingMemory.Augend.CurrentDigit == null && 
                    this.WorkingMemory.Addend.CurrentDigit == null
                )
                    this.Complete();
            }

            return targetHandled;
        }

        private void Complete()
        {
            this.completionCallback(this, [.. this.WorkingMemory.GetSumNeurons()]);
            this.WorkingMemory.Sum.Value.Items.Clear();
        }
    }
}
