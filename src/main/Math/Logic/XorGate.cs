namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class XorGate : DualInputLogicGateBase
    {
        protected override Neuron[] GetInterneuronOutputs(BinaryNeuronInfo output) =>
        [
            output.Neuron0,
            output.Neuron1,
            output.Neuron1,
            output.Neuron0
        ];
    }
}
