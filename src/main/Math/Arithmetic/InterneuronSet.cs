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

        // TODO: strongly-type based on variables in Adder.CreateInterneuronNetworksCore
        public IEnumerable<ReadOnlyNetwork> Interneurons = interneurons;
    }
}
