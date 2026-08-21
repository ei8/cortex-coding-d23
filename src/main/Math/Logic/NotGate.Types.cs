using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public partial class NotGate
    {
        public class Input(
            BinaryNeuronParameter? input1
        ) :
        InputCircuitParameterSubset<BinaryNeuronParameter>(
            input1
        )
        {
            public BinaryNeuronParameter? Input1 => this.Parameter1;
        }

        public class Output(
            BinaryNeuronParameter? output1
        ) :
        OutputCircuitParameterSubset<BinaryNeuronParameter>(
            output1
        )
        {
            public BinaryNeuronParameter? Output1 => this.Parameter1;
        }

        public class InterneuronSet
        (
            ReadOnlyNetwork interneuron1,
            ReadOnlyNetwork interneuron2,
            ReadOnlyNetwork linkedInputNeurons
        ) :
            CircuitInterneuronSetBase,
            ICircuitInterneuronSet
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            [
                this.Interneuron1,
                this.Interneuron2,
                this.LinkedInputNeurons
            ];

            public ReadOnlyNetwork Interneuron1 = interneuron1;
            public ReadOnlyNetwork Interneuron2 = interneuron2;
            public ReadOnlyNetwork LinkedInputNeurons = linkedInputNeurons;
        }
    }
}
