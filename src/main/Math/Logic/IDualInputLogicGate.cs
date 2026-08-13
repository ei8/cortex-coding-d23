using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public interface IDualInputLogicGate<T> : ILogicGate<T>
        where T : ICircuit<FunctionalParameter<BinaryNeuronParameter>, BinaryNeuronParameter>
    {
        static abstract IEnumerable<ReadOnlyNetwork> LinkInputNeurons(
            BinaryNeuronParameter input1,
            BinaryNeuronParameter input2,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            params Neuron[] additionalInputs
        );
    }
}
