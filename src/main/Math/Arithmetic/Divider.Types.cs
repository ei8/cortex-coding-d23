using ei8.Cortex.Coding.d23.Math.Logic;
using System.Collections.Generic;

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
            BinaryNeuronParameter dividend,
            BinaryNeuronParameter divisor
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
            public BinaryNeuronParameter Dividend => this.Parameter1;
            public BinaryNeuronParameter Divisor => this.Parameter2;
        }

        public class Output
        (
            BinaryNeuronParameter quotient,
            BinaryNeuronParameter undefined
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
            public BinaryNeuronParameter Quotient => this.Parameter1;
            public BinaryNeuronParameter Undefined => this.Parameter2;
        }

        public class InterneuronSet : CircuitInterneuronSetBase
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
                [
                    ..NetworkHelper.ConvertToNetworks
                    (
                        this.AND___Dividend__Divisor
                    ),
                    this.InterneuronsToUndefined
                ];
                
            public required AndGate AND___Dividend__Divisor { get; set; }

            public required ReadOnlyNetwork InterneuronsToUndefined { get; set; }
        }
    }
}
