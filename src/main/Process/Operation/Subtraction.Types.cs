using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class Subtraction
    {
        public class WorkingMemoryInfo
        (
            IEnumerableChunk<INeuronChunk> precedingBorrowValues,
            IOperandChunk minuend,
            IOperandChunk subtrahend,
            IResultChunk difference,
            IResultChunk borrow
        ) :
            WorkingMemoryBase
            <
                IEnumerableChunk<INeuronChunk>,
                IOperandChunk,
                IOperandChunk,
                IResultChunk,
                IResultChunk
            >
            (
                precedingBorrowValues,
                minuend,
                subtrahend,
                difference,
                borrow
            ),
            ISyncOperationWorkingMemory
        {
            private INeuronChunk[]? precedingBorrowValuesArray = null;

            public IEnumerable<INeuronChunk> GetCurrent(int digitIndex)
            {
                IEnumerable<INeuronChunk> result;

                if
                (
                    this.PrecedingBorrowValuesArray.TryGetDigitValueByGroupedValueIndex
                    (
                        this.Borrow.Value,
                        digitIndex,
                        out var precedingBorrow
                    ) &&
                    this.Minuend.TryGetDigitValueByValueIndex(digitIndex, out var minuend) &&
                    this.Subtrahend.TryGetDigitValueByValueIndex(digitIndex, out var subtrahend)
                )
                    result = 
                        [
                            precedingBorrow,
                            minuend, 
                            subtrahend
                        ];
                else
                    throw new ArgumentOutOfRangeException(nameof(digitIndex));

                return result;
            }

            public IOperandChunk[] GetOperands() =>
                [
                    this.Minuend,
                    this.Subtrahend
                ];

            public IResultChunk[] GetResults() =>
                [
                    this.Difference,
                    this.Borrow
                ];

            public IEnumerableChunk<INeuronChunk> PrecedingBorrowValues => this.Content1;

            private INeuronChunk[] PrecedingBorrowValuesArray
            {
                get
                {
                    this.precedingBorrowValuesArray ??= [.. this.PrecedingBorrowValues.Items];

                    return this.precedingBorrowValuesArray;
                }
            }

            public IOperandChunk Minuend => this.Content2;

            public IOperandChunk Subtrahend => this.Content3;

            public IResultChunk Difference => this.Content4;

            public IResultChunk Borrow => this.Content5;
        }
    }
}
