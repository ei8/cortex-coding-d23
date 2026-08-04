using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class ImplyGate : DualInputLogicGateBase
    {
        protected override IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output) =>
        [
            output.Neuron1,
            output.Neuron1,
            output.Neuron0,
            output.Neuron1
        ];
    }
}
