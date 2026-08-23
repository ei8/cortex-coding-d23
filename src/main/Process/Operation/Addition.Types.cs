using System;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class Addition
    {
        public class WorkingMemoryInfo
        (
            EnumerableChunk precedingCarryOverValues,
            EnumerableChunk addend1Values,
            EnumerableChunk addend2Values,
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
                EnumerableChunk precedingCarryOverValues,
                EnumerableChunk addend1Values,
                EnumerableChunk addend2Values,
                EnumerableChunk addend1Digits,
                EnumerableChunk addend2Digits,
                EnumerableChunk sumValues,
                EnumerableChunk carryOverValues
            ) : 
                this
                (
                    precedingCarryOverValues,
                    addend1Values,
                    addend2Values,
                    addend1Digits,
                    addend2Digits,
                    sumValues,
                    new(),
                    carryOverValues,
                    new()
                )
            {
                ArgumentOutOfRangeException.ThrowIfNotEqual(addend1Digits.Content.Count(), addend2Digits.Content.Count());
            }

            public EnumerableChunk PrecedingCarryOverValues => this.Chunk1;

            public EnumerableChunk Addend1Values => this.Chunk2;

            public EnumerableChunk Addend2Values => this.Chunk3;

            public EnumerableChunk Addend1Digits => this.Chunk4;

            public EnumerableChunk Addend2Digits => this.Chunk5;

            public EnumerableChunk SumValues => this.Chunk6;

            public ListChunk Sums => this.Chunk7;

            public EnumerableChunk CarryOverValues => this.Chunk8;

            public WriteableNullableNeuronChunk CarryOver => this.Chunk9;
        }
    }
}
