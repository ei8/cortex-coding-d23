namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Adder
    {
        public class Input
        (
            BinaryNeuronParameter? addend1,
            BinaryNeuronParameter? addend2,
            BinaryNeuronParameter? precedingCarryOver
        ) :
            InputCircuitParameterSubset<BinaryNeuronParameter, BinaryNeuronParameter, BinaryNeuronParameter>(
                addend1,
                addend2,
                precedingCarryOver
            )
        {
            public BinaryNeuronParameter? Addend1 => this.Parameter1;
            public BinaryNeuronParameter? Addend2 => this.Parameter2;
            public BinaryNeuronParameter? PrecedingCarryOver => this.Parameter3;
        }

        public class Output
        (
            BinaryNeuronParameter? sum,
            BinaryNeuronParameter? carryOver
        ) :
            OutputCircuitParameterSubset<BinaryNeuronParameter, BinaryNeuronParameter>(
                sum,
                carryOver
            )
        {
            public BinaryNeuronParameter? Sum => this.Parameter1;
            public BinaryNeuronParameter? CarryOver => this.Parameter2;
        }
    }
}
