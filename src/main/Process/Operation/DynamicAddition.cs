using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public class DynamicAddition
    (
        Addition.WorkingMemoryInfo workingMemory,
        Action<DynamicAddition, IEnumerable<Neuron>> completionCallback
    ) :
        SyncOperationFiniteProcessBase
        <
            DynamicAddition,
            Addition.WorkingMemoryInfo
        >
        (
            workingMemory,
            completionCallback
        )
    {
        protected override bool IsTrackingDirectionNext => true;

        protected override bool IsDigitProgressionIncremental => true;

        protected override IEnumerable<Neuron> BuildResults() =>
            [.. this.WorkingMemory.GetSumNeurons()];
    }
}
