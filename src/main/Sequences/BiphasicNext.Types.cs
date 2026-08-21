using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Sequences
{
    public partial class BiphasicNext
    {
        public class InterneuronSet
        (
            ReadOnlyNetwork initiator,
            ReadOnlyNetwork completer
        ) :
            CircuitInterneuronSetBase,
            ICircuitInterneuronSet
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            [
                this.Initiator,
                this.Completer
            ];

            public ReadOnlyNetwork Initiator { get; } = initiator;
            public ReadOnlyNetwork Completer { get; } = completer;
        }
    }
}
