namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public class In2Out3TruthTable
    {
        public class ParameterInfo
        (
            In2TruthTableBase.Input inputs,
            Output outputs
        ) :
            FunctionalCircuitParameter
            <
                In2TruthTableBase.Input,
                Output
            >
            (
                inputs,
                outputs
            )
        {
        }
        public class Output
        (
            BinaryNeuronParameter? output1,
            BinaryNeuronParameter? output2,
            BinaryNeuronParameter? output3
        ) : 
            OutputCircuitParameterSubsetBase
            <
                BinaryNeuronParameter,
                BinaryNeuronParameter,
                BinaryNeuronParameter
            >
            (
                output1,
                output2,
                output3
            )
        {
            public BinaryNeuronParameter? Output1 => this.Parameter1;

            public BinaryNeuronParameter? Output2 => this.Parameter2;

            public BinaryNeuronParameter? Output3 => this.Parameter3;
        }
    }
}
