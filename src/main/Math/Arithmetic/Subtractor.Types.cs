namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Subtractor
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

        public class Input(
            BinaryNeuronParameter? minuend,
            BinaryNeuronParameter? subtrahend,
            BinaryNeuronParameter? precedingBorrow
        ) :
            InputCircuitParameterSubsetBase<BinaryNeuronParameter, BinaryNeuronParameter, BinaryNeuronParameter>(
                minuend,
                subtrahend,
                precedingBorrow
            )
        {
            public BinaryNeuronParameter? Minuend => this.Parameter1;
            public BinaryNeuronParameter? Subtrahend => this.Parameter2;
            public BinaryNeuronParameter? PrecedingBorrow => this.Parameter3;
        }

        public class Output(
            BinaryNeuronParameter? difference,
            BinaryNeuronParameter? borrow
        ) :
        OutputCircuitParameterSubsetBase<BinaryNeuronParameter, BinaryNeuronParameter>(
            difference,
            borrow
        )
        {
            public BinaryNeuronParameter? Difference => this.Parameter1;
            public BinaryNeuronParameter? Borrow => this.Parameter2;
        }
    }
}
