using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Iteration
{
    public partial class DoUntil
    {
        public class WorkingMemoryInfo
        (
            ReadOnlyNeuronChunk action,
            EnumerableChunk counterVariableValues,
            WriteableNeuronChunk counterVariable,
            ReadOnlyNeuronChunk condition
        ) :
            WorkingMemoryBase
            <
                ReadOnlyNeuronChunk,
                EnumerableChunk,
                WriteableNeuronChunk,
                ReadOnlyNeuronChunk
            >
            (
                action,
                counterVariableValues,
                counterVariable,
                condition
            )
        {
            public WorkingMemoryInfo
            (
                ReadOnlyNeuronChunk action,
                EnumerableChunk counterVariableValues,
                WriteableNeuronChunk counterVariable
            ) : 
                this
                (
                    action,
                    counterVariableValues,
                    counterVariable,
                    new(counterVariableValues.Content.Last())
                )
            {
            }

            public ReadOnlyNeuronChunk Action => this.Chunk1;
            public EnumerableChunk CounterVariableValues => this.Chunk2;
            public WriteableNeuronChunk CounterVariable => this.Chunk3;
            public ReadOnlyNeuronChunk Condition => this.Chunk4;
        }
    }
}
