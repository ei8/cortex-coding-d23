namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Divider
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
            BinaryNeuronParameter? dividend,
            BinaryNeuronParameter? divisor
        ) :
            InputCircuitParameterSubsetBase
            <
                BinaryNeuronParameter, 
                BinaryNeuronParameter
            >
            (
                dividend,
                divisor
            )
        {
            public BinaryNeuronParameter? Dividend => this.Parameter1;
            public BinaryNeuronParameter? Divisor => this.Parameter2;
        }

        public class Output
        (
            BinaryNeuronParameter? quotient,
            BinaryNeuronParameter? undefined
        ) :
            OutputCircuitParameterSubsetBase
            <
                BinaryNeuronParameter,
                BinaryNeuronParameter
            >
            (
                quotient,
                undefined
            )
        {
            public BinaryNeuronParameter? Quotient => this.Parameter1;
            public BinaryNeuronParameter? Undefined => this.Parameter2;
        }
    }
}
