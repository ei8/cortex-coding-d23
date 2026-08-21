namespace ei8.Cortex.Coding.d23.Process.Iteration
{
    public partial class DoUntil
    {
        public class WorkingMemoryInfo
        (
            ReadOnlyNeuronChunk action,
            WriteableNeuronChunk counterVariable,
            ReadOnlyNeuronChunk condition
        ) :
            WorkingMemoryBase
            <
                ReadOnlyNeuronChunk,
                WriteableNeuronChunk,
                ReadOnlyNeuronChunk
            >
            (
                action,
                counterVariable,
                condition
            )
        {
            public ReadOnlyNeuronChunk Action => this.Chunk1;
            public WriteableNeuronChunk CounterVariable => this.Chunk2;
            public ReadOnlyNeuronChunk Condition => this.Chunk3;
        }
    }
}
