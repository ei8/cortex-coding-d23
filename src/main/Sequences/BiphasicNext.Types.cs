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
            CircuitInterneuronSetBase
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
                [
                    this.Initiator,
                    this.Completer
                ];

            public ReadOnlyNetwork Initiator => initiator;
            public ReadOnlyNetwork Completer => completer;
        }
    }
}
