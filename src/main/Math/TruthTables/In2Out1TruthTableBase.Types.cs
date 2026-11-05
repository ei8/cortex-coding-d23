namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    // TODO: Remove Base from filename and class name
    public class In2Out1TruthTableBase
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
        public class Output(BinaryNeuronParameter? output1) : OutputCircuitParameterSubsetBase<BinaryNeuronParameter>(output1)
        {
            public BinaryNeuronParameter? Output1 => this.Parameter1;
        }
    }
}
