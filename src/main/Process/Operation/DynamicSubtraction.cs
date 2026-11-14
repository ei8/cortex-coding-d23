using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public class DynamicSubtraction
    (
        Subtraction.WorkingMemoryInfo workingMemory,
        Action<DynamicSubtraction, IEnumerable<Neuron>> completionCallback
    ) :
        SyncOperationFiniteProcessBase
        <
            DynamicSubtraction,
            Subtraction.WorkingMemoryInfo
        >
        (
            workingMemory,
            completionCallback
        )
    {
        protected override bool IsTrackingDirectionNext => true;

        protected override bool IsDigitProgressionIncremental => true;

        protected override IEnumerable<Neuron> BuildResults()
        {
            var result = new List<Neuron>(this.WorkingMemory.Difference.Value.Items.Select(c => c.Neuron));

            if
            (
                this.WorkingMemory.Borrow != null &&
                this.WorkingMemory.Borrow.Value.Items.Last().Neuron.Tag.EndsWith('1')
            )
                result.Add(this.WorkingMemory.Borrow.Value.Items.Last().Neuron);

            return result;
        }
    }
}
