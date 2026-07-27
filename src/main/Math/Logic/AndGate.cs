namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class AndGate : DualInputLogicGateBase
    {
        protected override Neuron[] GetInterneuronOutputs(BinaryNeuronInfo output) =>
        [
            output.Neuron0, 
            output.Neuron0, 
            output.Neuron0, 
            output.Neuron1
        ];
    }
}
