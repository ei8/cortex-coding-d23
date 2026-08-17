using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class SequentialInterneuronSet
    (
        ReadOnlyNetwork initiator,
        InterneuronSet innerInterneurons,
        ReadOnlyNetwork completer
    ):
        CircuitInterneuronSetBase,
        ICircuitInterneuronSet
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => 
        [
            this.Initiator,
            this.innerInterneurons.Network,
            this.Completer
        ];

        public ReadOnlyNetwork Initiator = initiator;

        public InterneuronSet innerInterneurons = innerInterneurons;

        public ReadOnlyNetwork Completer = completer;
    }
}
