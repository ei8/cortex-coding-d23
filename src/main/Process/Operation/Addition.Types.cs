using System;
using System.Collections.Generic;
using System.Linq;

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

            public static IEnumerable<Neuron> GetResultNeuronns(Addition.WorkingMemoryInfo workingMemory)
            {
                var result = new List<Neuron>(workingMemory.Sum.Value.Items.Select(c => c.Neuron));

                if
                (
                    workingMemory.CarryOver != null &&
                    workingMemory.CarryOver.Value.Items.Last().Neuron.Tag.EndsWith('1')
                )
                    result.Add(workingMemory.CarryOver.Value.Items.Last().Neuron);

                return result;
            }

            public IEnumerable<INeuronChunk> GetCurrent(int digitIndex)
            {
                var result = new List<INeuronChunk>();

                result.AddRange
                (
                    [
                        this.Augend.GetDigitValueByValueIndex(digitIndex),
                        this.Addend.GetDigitValueByValueIndex(digitIndex)
                    ]
                );

                var noMatchComparer = new Predicate<Neuron>(n => n.Tag.EndsWith('0'));
                if (digitIndex == 0)
                    result.Add(this.PrecedingCarryOverValuesArray.GetValueByComparer(noMatchComparer));
                else if (digitIndex - 1 < this.CarryOver.Value.Count)
                    result.Add
                    (
                        this.PrecedingCarryOverValuesArray.GetDigitValueByValueIndex
                        (
                            this.CarryOver.Value.Count,
                            digitIndex - 1,
                            (digitNeuron, valueNeuron) =>
                                digitNeuron.Tag.EndsWith(valueNeuron.Tag[^1]),
                            noMatchComparer,
                            this.CarryOver.Value.ItemAt
                        )
                    );

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
                    this.precedingCarryOverValuesArray ??= this.PrecedingCarryOverValues.Items.ToArray();

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
