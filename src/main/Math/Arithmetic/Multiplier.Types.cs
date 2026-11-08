using ei8.Cortex.Coding.d23.Math.Logic;
using System.Collections.Generic;

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

        public class InterneuronSet : CircuitInterneuronSetBase
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
                this.AND___Multiplicand__Multiplier != null ?
                    [this.AND___Multiplicand__Multiplier.Network] :
                    [];

            public AndGate? AND___Multiplicand__Multiplier { get; internal set; }
        }
    }
}
