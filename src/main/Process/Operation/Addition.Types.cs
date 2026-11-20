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

            public IEnumerable<INeuronChunk> GetCurrent(int digitIndex) =>
                [
                    this.PrecedingCarryOverValues.Items.Single
                    (
                        n =>
                            n.Neuron.Tag.EndsWith
                            (
                                digitIndex == 0 ?
                                    '0' :
                                    this.CarryOver.Value.ItemAt(digitIndex - 1).Neuron.Tag.Last()
                            )
                    ),
                    this.Augend.GetValueByIndex(digitIndex),
                    this.Addend.GetValueByIndex(digitIndex)
                ];

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

            public IOperandChunk Augend => this.Content2;

            public IOperandChunk Addend => this.Content3;

            public IResultChunk Sum => this.Content4;

            public IResultChunk CarryOver => this.Content5;
        }
    }
}
