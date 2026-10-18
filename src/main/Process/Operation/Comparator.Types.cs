using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public class Comparator
    {
        public class WorkingMemoryInfo
        (
            IOperandChunk comparand1,
            IOperandChunk comparand2,
            IResultChunk areEqual,
            IResultChunk isLessThan,
            IResultChunk isGreaterThan
        ) : 
            WorkingMemoryBase
            <
                IOperandChunk,
                IOperandChunk,
                IResultChunk,
                IResultChunk,
                IResultChunk
            >
            (
                comparand1,
                comparand2,
                areEqual,
                isLessThan,
                isGreaterThan
            )
        {
            public IEnumerable<INeuronChunk> GetCurrent(int digitIndex) =>
                [
                    this.Comparand1.GetValueByIndex(digitIndex),
                    this.Comparand2.GetValueByIndex(digitIndex)
                ];

            public IOperandChunk Comparand1 => this.Content1;

            public IOperandChunk Comparand2 => this.Content2;

            public IResultChunk AreEqual => this.Content3;

            public IResultChunk IsLessThan => this.Content4;

            public IResultChunk IsGreaterThan => this.Content5;
        }
    }
}
