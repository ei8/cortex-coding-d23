using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class SequentialAdder
    {
        public class AugmentationInfo(ReadOnlyNetwork interneuronPresynapticTerminals) : AugmentationBase
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => [this.InterneuronPresynapticTerminals];

            public ReadOnlyNetwork InterneuronPresynapticTerminals { get; } = interneuronPresynapticTerminals;
        }
    }
}
