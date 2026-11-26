using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class Addition
    {
        public class WorkingMemoryInfo
        (
            IEnumerableChunk<INeuronChunk> precedingCarryOverValues,
            IOperandChunk augend,
            IOperandChunk addend,
            IResultChunk sum,
            IResultChunk carryOver
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
                precedingCarryOverValues,
                augend,
                addend, 
                sum,
                carryOver
            ),
            ISyncOperationWorkingMemory
        {

            private INeuronChunk[]? precedingCarryOverValuesArray = null;

            public static IEnumerable<Neuron> GetResultNeurons(Addition.WorkingMemoryInfo workingMemory)
            {
                var result = new List<Neuron>(workingMemory.Sum.ValueArray.Select(c => c.Neuron));

                Neuron? lastCarryOverNeuron;
                if
                (
                    workingMemory.CarryOver != null &&
                    (lastCarryOverNeuron = workingMemory.CarryOver.ValueArray[^1].Neuron) != null &&
                    lastCarryOverNeuron.Tag.EndsWith('1')
                )
                    result.Add(lastCarryOverNeuron);

                return result;
            }

            public IEnumerable<INeuronChunk> GetCurrent(int digitIndex)
            {
                IEnumerable<INeuronChunk> result;

                if
                (
                    this.PrecedingCarryOverValuesArray.TryGetDigitValueByGroupedValueIndex
                    (
                        this.CarryOver.Value,
                        digitIndex,
                        out var precedingCarryOver
                    ) &&
                    this.Augend.TryGetDigitValueByValueIndex(digitIndex, out var augend) &&
                    this.Addend.TryGetDigitValueByValueIndex(digitIndex, out var addend)
                )
                    result =
                        [
                            precedingCarryOver,
                            augend,
                            addend
                        ];
                else
                    throw new ArgumentOutOfRangeException(nameof(digitIndex));

                return result;
            }

            public IOperandChunk[] GetOperands() =>
                [
                    this.Augend,
                    this.Addend
                ];

            public IResultChunk[] GetResults() =>
                [
                    this.Sum,
                    this.CarryOver
                ];

            public IEnumerableChunk<INeuronChunk> PrecedingCarryOverValues => this.Content1;

            private INeuronChunk[] PrecedingCarryOverValuesArray
            {
                get
                {
                    this.precedingCarryOverValuesArray ??= [.. this.PrecedingCarryOverValues.Items];

                    return this.precedingCarryOverValuesArray;
                }
            }

            public IOperandChunk Augend => this.Content2;

            public IOperandChunk Addend => this.Content3;

            public IResultChunk Sum => this.Content4;

            public IResultChunk CarryOver => this.Content5;
        }
    }
}
