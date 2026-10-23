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
        SyncOperationFiniteProcessBase
        <
            DynamicLessThan,
            Comparator.WorkingMemoryInfo
        >
        (
            workingMemory,
            completionCallback
        )
    {
        protected override bool IsTrackingDirectionNext => false;

        protected override bool IsDigitProgressionIncremental => false;

        protected override bool IsComplete() =>
            base.IsComplete() ||
            this.WorkingMemory.IsLessThan.Value.Items.Last().Neuron.Tag.EndsWith('1') ||
            this.WorkingMemory.IsGreaterThan.Value.Items.Last().Neuron.Tag.EndsWith('1');
       
        protected override IEnumerable<Neuron> BuildResults() =>
            [.. this.WorkingMemory.IsLessThan.Value.Items.Select(c => c.Neuron)];
    }
}
