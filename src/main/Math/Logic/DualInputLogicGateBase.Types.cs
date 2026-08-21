using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract partial class DualInputLogicGateBase
    {
        public class Input(
            BinaryNeuronParameter? input1,
            BinaryNeuronParameter? input2
        ) :
        InputCircuitParameterSubset<BinaryNeuronParameter, BinaryNeuronParameter>(
            input1,
            input2
        )
        {
            public BinaryNeuronParameter? Input1 => this.Parameter1;
            public BinaryNeuronParameter? Input2 => this.Parameter2;
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
            ReadOnlyNetwork interneuron3,
            ReadOnlyNetwork interneuron4,
            ReadOnlyNetwork linkedInputNeurons
        ) :
            CircuitInterneuronSetBase,
            ICircuitInterneuronSet
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            [
                this.Interneuron1,
                this.Interneuron2,
                this.Interneuron3,
                this.Interneuron4,
                this.LinkedInputNeurons
            ];

            public ReadOnlyNetwork Interneuron1 = interneuron1;
            public ReadOnlyNetwork Interneuron2 = interneuron2;
            public ReadOnlyNetwork Interneuron3 = interneuron3;
            public ReadOnlyNetwork Interneuron4 = interneuron4;
            public ReadOnlyNetwork LinkedInputNeurons = linkedInputNeurons;
        }
    }
}
