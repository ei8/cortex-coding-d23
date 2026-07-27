namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class XnorGate : DualInputLogicGateBase
    {
        protected override Neuron[] GetInterneuronOutputs(BinaryNeuronInfo output) =>
        [
            output.Neuron1,
            output.Neuron0,
            output.Neuron0,
            output.Neuron1
        ];
    }
}
