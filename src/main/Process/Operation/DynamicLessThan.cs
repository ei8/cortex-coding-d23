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

        public override bool HandleFire(Neuron targetNeuron)
        {
            // TODO: Refactor with DynamicAddition
            var result = false;
            if
            (
                (
                    this.WorkingMemory.Comparand1.HasCurrentDigitChanged() ||
                    this.WorkingMemory.Comparand2.HasCurrentDigitChanged()
                ) &&
                this.WorkingMemory.TryHandleAllResults
                (
                    targetNeuron,
                    this.WorkingMemory.Comparand1,
                    this.WorkingMemory.Comparand2,
                    false,
                    out result
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
            return result;
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
