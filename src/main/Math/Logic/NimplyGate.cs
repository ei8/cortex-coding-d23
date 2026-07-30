using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NimplyGate : DualInputLogicGateBase
    {
        protected override IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronInfo output) =>
        [
            output.Neuron0,
            output.Neuron0,
            output.Neuron1,
            output.Neuron0
        ];
    }
}
