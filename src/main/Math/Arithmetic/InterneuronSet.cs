using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class InterneuronSet
    (
        IEnumerable<ReadOnlyNetwork> interneurons
    ) :
        CircuitInterneuronSetBase,
        ICircuitInterneuronSet
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => this.Interneurons;

        public IEnumerable<ReadOnlyNetwork> Interneurons = interneurons;
    }
}
