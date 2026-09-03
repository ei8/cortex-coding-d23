using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class DynamicMultiplication
    {
        public class WorkingMemoryInfo
        (
            EnumerableChunk<NeuronChunk> multiplicandValues,
            EnumerableChunk<NeuronChunk> multiplierValues,
            EnumerableChunk<NeuronChunk> multiplicand,
            EnumerableChunk<NeuronChunk> multiplier,
            NeuronChunk? currentMultiplicandDigit,
            NeuronChunk? currentMultiplierDigit,
            NeuronChunk? lastMultiplicandDigit,
            EnumerableChunk<NeuronChunk> productValues,
            NestedListChunk<NeuronChunk> multiplierProducts,
            IListChunk<NeuronChunk>? currentMultiplierProduct,
            IListChunk<NeuronChunk>? lastAdditionSum,
            ListChunk<NeuronChunk> product
        ) :
            IWorkingMemory
        {
            public WorkingMemoryInfo
            (
                EnumerableChunk<NeuronChunk> multiplicandValues,
                EnumerableChunk<NeuronChunk> multiplierValues,
                EnumerableChunk<NeuronChunk> multiplicand,
                EnumerableChunk<NeuronChunk> multiplier,
                EnumerableChunk<NeuronChunk> productValues
            ) :
                this
                (
                    multiplicandValues,
                    multiplierValues,
                    multiplicand,
                    multiplier,
                    multiplicand.Content.First(),
                    multiplier.Content.First(),
                    null,
                    productValues,
                    new(),
                    null,
                    null,
                    new()
                )
            {
            }

            public int GetCurrentMultiplierProductIndex() =>
                this.CurrentMultiplierProduct != null ?
                    this.MultiplierProducts.Content.IndexOf(this.CurrentMultiplierProduct) : 
                    -1;

            public EnumerableChunk<NeuronChunk> MultiplicandValues => multiplicandValues;

            public EnumerableChunk<NeuronChunk> MultiplierValues => multiplierValues;

            public EnumerableChunk<NeuronChunk> Multiplicand => multiplicand;

            public EnumerableChunk<NeuronChunk> Multiplier => multiplier;

            public NeuronChunk? CurrentMultiplicandDigit { get; set; } = currentMultiplicandDigit;

            public NeuronChunk? CurrentMultiplierDigit { get; set; } = currentMultiplierDigit;

            public NeuronChunk? LastMultiplicandDigit { get; set; } = lastMultiplicandDigit;

            public EnumerableChunk<NeuronChunk> ProductValues => productValues;

            public NestedListChunk<NeuronChunk> MultiplierProducts => multiplierProducts;

            public IListChunk<NeuronChunk>? CurrentMultiplierProduct { get; set; } = currentMultiplierProduct;
            
            public IListChunk<NeuronChunk>? LastAdditionSum { get; set; } = lastAdditionSum;

            public ListChunk<NeuronChunk> Product => product;
        }
    }
}