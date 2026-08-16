using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Collections
{
    public class InterneuronSet
        (
            ReadOnlyNetwork interneuron1,
            ReadOnlyNetwork linkedInputNeurons
        ) :
            CircuitInterneuronSetBase,
            ICircuitInterneuronSet
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
        [
            this.Interneuron1,
            this.LinkedInputNeurons
        ];

        public ReadOnlyNetwork Interneuron1 = interneuron1;
        public ReadOnlyNetwork LinkedInputNeurons = linkedInputNeurons;
    }
}
