using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Sequences
{
    public partial class Next
    {
        public class Input
        (
            UnaryNeuronParameter? function,
            UnaryNeuronParameter? current
        ) :
            InputCircuitParameterSubset
            <
                UnaryNeuronParameter,
                UnaryNeuronParameter
            >
            (
                function,
                current
            )
        {
            public UnaryNeuronParameter? Function => this.Parameter1;
            public UnaryNeuronParameter? Current => this.Parameter2;
        }

        public class Output(UnaryNeuronParameter? subsequent) : OutputCircuitParameterSubset<UnaryNeuronParameter>(subsequent)
        {
            public UnaryNeuronParameter? Next => this.Parameter1;
        }

        public class InterneuronSet
        (
            ReadOnlyNetwork interneuron
        ) :
            CircuitInterneuronSetBase,
            ICircuitInterneuronSet
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            [
                this.Interneuron
            ];

            public ReadOnlyNetwork Interneuron { get; } = interneuron;
        }
    }
}
