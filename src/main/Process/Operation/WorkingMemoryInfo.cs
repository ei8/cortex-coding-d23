namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public class WorkingMemoryInfo
    (
        EnumerableChunk precedingCarryOverValues,
        EnumerableChunk addend1Values,
        EnumerableChunk addend2Values,
        EnumerableChunk sumValues,
        ListChunk sums,
        EnumerableChunk carryOverValues,
        WriteableNullableNeuronChunk carryOver
    ) :
        WorkingMemoryBase
        <
            EnumerableChunk,
            EnumerableChunk,
            EnumerableChunk,
            EnumerableChunk,
            ListChunk,
            EnumerableChunk,
            WriteableNullableNeuronChunk
        >
        (
            precedingCarryOverValues,
            addend1Values,
            addend2Values,
            sumValues,
            sums,
            carryOverValues,
            carryOver
        )
    {
        public WorkingMemoryInfo
        (
            EnumerableChunk precedingCarryOverValues,
            EnumerableChunk addend1Values,
            EnumerableChunk addend2Values,
            EnumerableChunk sumValues,
            EnumerableChunk carryOverValues
        ) :
            this
            (
                precedingCarryOverValues,
                addend1Values,
                addend2Values,
                sumValues,
                new(),
                carryOverValues,
                new()
            )
        {
        }

        public EnumerableChunk PrecedingCarryOverValues => this.Chunk1;

        public EnumerableChunk Addend1Values => this.Chunk2;

        public EnumerableChunk Addend2Values => this.Chunk3;

        public EnumerableChunk SumValues => this.Chunk4;

        public ListChunk Sums => this.Chunk5;

        public EnumerableChunk CarryOverValues => this.Chunk6;

        public WriteableNullableNeuronChunk CarryOver => this.Chunk7;
    }
}