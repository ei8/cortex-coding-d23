using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Collections
{
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

        public ReadOnlyNetwork Interneuron = interneuron;
    }
}
