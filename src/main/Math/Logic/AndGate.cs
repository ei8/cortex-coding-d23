using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class AndGate : DualInputLogicGateBase
    {
        protected override IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output) =>
        [
            output.Neuron0, 
            output.Neuron0, 
            output.Neuron0, 
            output.Neuron1
        ];
    }
}
