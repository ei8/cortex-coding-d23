using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class DynamicMultiplication
    {
        public class WorkingMemoryInfo
        (
            IOperandChunk multiplicand,
            IOperandChunk multiplier,
            INestedNeuronListChunk<IListChunk<INeuronChunk>> multiplierProducts,
            IListChunk<INeuronChunk>? currentMultiplierProduct,
            IListChunk<INeuronChunk>? lastAdditionSum,
            IResultChunk product
        ) :
            WorkingMemoryBase
            <
                IOperandChunk,
                IOperandChunk,
                INestedNeuronListChunk<IListChunk<INeuronChunk>>,
                IListChunk<INeuronChunk>?,
                IListChunk<INeuronChunk>?,
                IResultChunk
            >
            (
                multiplicand,
                multiplier,
                multiplierProducts,
                currentMultiplierProduct,
                lastAdditionSum,
                product
            ),
            IAsyncOperationWorkingMemory
        {
            public IEnumerable<INeuronChunk> GetCurrent()
            {
                var result = new List<INeuronChunk>();
                if
                (
                    this.Multiplicand.CurrentDigit != null &&
                    this.Multiplier.CurrentDigit != null
                )
                    result.AddRange
                    (
                        [
                            this.Multiplicand.DigitValues.Items.Single(mv => mv.Neuron.Tag.EndsWith(this.Multiplicand.CurrentDigit.Neuron.Tag.Last())),
                            this.Multiplier.DigitValues.Items.Single(mv => mv.Neuron.Tag.EndsWith(this.Multiplier.CurrentDigit.Neuron.Tag.Last()))
                        ]
                    );

                return result;
            }

            public IOperandChunk Multiplicand => this.Content1;

            public IOperandChunk Multiplier => this.Content2;

            public INestedNeuronListChunk<IListChunk<INeuronChunk>> MultiplierProducts => this.Content3;

            public IListChunk<INeuronChunk>? CurrentMultiplierProduct { get => this.Content4; set => this.Content4 = value; }

            public IListChunk<INeuronChunk>? LastAdditionSum { get => this.Content5; set => this.Content5 = value; }

            public IResultChunk Product => this.Content6;
        }
    }
}