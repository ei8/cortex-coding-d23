using System.Collections.Generic;
using System.Linq;

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
            public IEnumerable<INeuronChunk> GetCurrent(int digitIndex) =>
                [
                    this.PrecedingBorrowValues.Items.Single
                    (
                        n =>
                            n.Neuron.Tag.EndsWith
                            (
                                digitIndex == 0 ?
                                    '0' :
                                    this.Borrow.Value.ItemAt(digitIndex - 1).Neuron.Tag.Last()
                            )
                    ),
                    this.Minuend.GetDigitValueByValueIndex(digitIndex),
                    this.Subtrahend.GetDigitValueByValueIndex(digitIndex)
                ];

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

            public IOperandChunk Minuend => this.Content2;

            public IOperandChunk Subtrahend => this.Content3;

            public IResultChunk Difference => this.Content4;

            public IResultChunk Borrow => this.Content5;
        }
    }
}
