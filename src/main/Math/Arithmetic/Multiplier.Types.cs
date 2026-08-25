namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Multiplier
    {
        public class Input
        (
            BinaryNeuronParameter? multiplicand,
            BinaryNeuronParameter? multiplier
        ) :
            InputCircuitParameterSubset<BinaryNeuronParameter, BinaryNeuronParameter>(
                multiplicand,
                multiplier
            )
        {
            public BinaryNeuronParameter? Multiplicand => this.Parameter1;
            public BinaryNeuronParameter? Multiplier => this.Parameter2;
        }

        public class Output
        (
            BinaryNeuronParameter? product
        ) :
            OutputCircuitParameterSubset<BinaryNeuronParameter>(
                product
            )
        {
            public BinaryNeuronParameter? Product => this.Parameter1;
        }
    }
}
