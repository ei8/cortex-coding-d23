using ei8.Cortex.Coding.d23.Math.TruthTables;

namespace ei8.Cortex.Coding.d23.Math.Comparison
{
    public partial class Comparator
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

        public class Output
        (
            BinaryNeuronParameter? areEqual,
            BinaryNeuronParameter? isLessThan,
            BinaryNeuronParameter? isGreaterThan
        ) :
            OutputCircuitParameterSubsetBase
            <
                BinaryNeuronParameter,
                BinaryNeuronParameter,
                BinaryNeuronParameter
            >
            (
                areEqual,
                isLessThan,
                isGreaterThan
            )
        {
            public BinaryNeuronParameter? AreEqual => this.Parameter1;

            public BinaryNeuronParameter? IsLessThan => this.Parameter2;

            public BinaryNeuronParameter? IsGreaterThan => this.Parameter3;
        }
    }
}
