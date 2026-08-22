namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class Addition
    {
        public class WorkingMemoryInfo
        (
            EnumerableChunk addend1Digits,
            EnumerableChunk addend2Digits,
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
                ListChunk,
                EnumerableChunk,
                WriteableNullableNeuronChunk
            >
            (
                addend1Digits,
                addend2Digits,
                sumValues,
                sums,
                carryOverValues,
                carryOver
            )
        {
            public WorkingMemoryInfo
            (
                EnumerableChunk addend1Digits,
                EnumerableChunk addend2Digits,
                EnumerableChunk sumValues,
                EnumerableChunk carryOverValues
            ) : 
                this
                (
                    addend1Digits,
                    addend2Digits,
                    sumValues,
                    new(),
                    carryOverValues,
                    new()
                )
            {
            }

            public EnumerableChunk Addend1Digits => this.Chunk1;

            public EnumerableChunk Addend2Digits => this.Chunk2;

            public EnumerableChunk SumValues => this.Chunk3;

            public ListChunk Sums => this.Chunk4;

            public EnumerableChunk CarryOverValues => this.Chunk5;

            public WriteableNullableNeuronChunk CarryOver => this.Chunk6;
        }
    }
}
