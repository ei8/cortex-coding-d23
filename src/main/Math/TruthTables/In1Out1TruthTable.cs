namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public class In1Out1TruthTable
    {
        public class ParameterInfo
        (
            In1TruthTableBase.Input inputs,
            Output outputs
        ) :
            FunctionalCircuitParameter
            <
                In1TruthTableBase.Input,
                Output
            >
            (
                inputs,
                outputs
            )
        {
        }

        public class Output(BinaryNeuronParameter output1) : OutputCircuitParameterSubsetBase<BinaryNeuronParameter>(output1)
        {
            public BinaryNeuronParameter Output1 => this.Parameter1;
        }
    }
}
