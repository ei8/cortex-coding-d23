using ei8.Cortex.Coding.d23.Process.Iteration;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class Addition
    {
        public class WorkingMemoryInfo(
            DoUntil.WorkingMemoryInfo doUntil,
            EnumerableChunk addend1Digits,
            EnumerableChunk addend2Digits,
            EnumerableChunk sumValues,
            ListChunk sums,
            EnumerableChunk carryOverValues,
            WriteableNullableNeuronChunk carryOver
        ) :
            WorkingMemoryBase
            <
                DoUntil.WorkingMemoryInfo,
                EnumerableChunk,
                EnumerableChunk,
                EnumerableChunk,
                ListChunk,
                EnumerableChunk,
                WriteableNullableNeuronChunk
            >
            (
                doUntil,
                addend1Digits,
                addend2Digits,
                sumValues,
                sums,
                carryOverValues,
                carryOver
            )
        {
            public DoUntil.WorkingMemoryInfo DoUntil => this.Chunk1;

            public EnumerableChunk Addend1Digits => this.Chunk2;

            public EnumerableChunk Addend2Digits => this.Chunk3;

            public EnumerableChunk SumValues => this.Chunk4;

            public ListChunk Sums => this.Chunk5;

            public EnumerableChunk CarryOverValues => this.Chunk6;

            public WriteableNullableNeuronChunk CarryOver => this.Chunk7;
        }
    }
}
