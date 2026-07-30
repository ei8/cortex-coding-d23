using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NandGate : DualInputLogicGateBase
    {
        protected override IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronInfo output) =>
        [
            output.Neuron1,
            output.Neuron1,
            output.Neuron1,
            output.Neuron0
        ];
    }
}
