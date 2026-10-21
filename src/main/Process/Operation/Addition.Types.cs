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
            public IEnumerable<Neuron> GetSumNeurons()
            {
                var result = new List<Neuron>(this.Sum.Value.Items.Select(c => c.Neuron));

                if (this.CarryOver != null && this.CarryOver.Value.Items.Last().Neuron.Tag.EndsWith('1'))
                    result.Add(this.CarryOver.Value.Items.Last().Neuron);

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
                                    this.CarryOver.Value.Items[digitIndex - 1].Neuron.Tag.Last()
                            )
                    ),
                    this.Augend.GetValueByIndex(digitIndex),
                    this.Addend.GetValueByIndex(digitIndex)
                ];

            public IEnumerableChunk<INeuronChunk> PrecedingCarryOverValues => this.Content1;

            public IOperandChunk Augend => this.Content2;

            public IOperandChunk Addend => this.Content3;

            public IResultChunk Sum => this.Content4;

            public IResultChunk CarryOver => this.Content5;
        }
    }
}
