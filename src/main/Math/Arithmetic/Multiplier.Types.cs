namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Multiplier
    {
        public class ParameterInfo
        (
            Input inputs,
            Output outputs
        ) :
            FunctionalCircuitParameter
            <
                Input,
                Output
            >
            (
                inputs,
                outputs
            )
        {
        }

        public class Input
        (
            BinaryNeuronParameter? multiplicand,
            BinaryNeuronParameter? multiplier
        ) :
            InputCircuitParameterSubsetBase<BinaryNeuronParameter, BinaryNeuronParameter>(
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
            OutputCircuitParameterSubsetBase<BinaryNeuronParameter>(
                product
            )
        {
            public BinaryNeuronParameter? Product => this.Parameter1;
        }
    }
}
