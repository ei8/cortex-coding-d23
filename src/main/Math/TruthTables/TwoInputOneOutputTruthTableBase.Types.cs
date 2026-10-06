namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public abstract partial class TwoInputOneOutputTruthTableBase
    {
        public class ParameterInfo
        (
            TwoInputTruthTableBase.Input inputs,
            Output outputs
        ) :
            FunctionalCircuitParameter
            <
                TwoInputTruthTableBase.Input,
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
