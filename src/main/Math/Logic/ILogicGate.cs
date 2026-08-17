using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public interface ILogicGate<T, TParam, TInterneuron> : ICircuit<TParam, TInterneuron>
        where T : ICircuit<TParam, TInterneuron>
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        static abstract IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output);

        static abstract IEnumerable<string> GetInterneuronTags(
            VariableInfo variableInfo,
            InterneuronTagInfo? interneuronTagInfo = null
        );

        static abstract T Create(
            TParam parameters,
            TInterneuron interneurons,
            VariableInfo? variableInfo
        );

        static abstract IEnumerable<ReadOnlyNetwork> LinkInputNeurons(
            TParam parameters,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            NetworkHelper.AdditionalInputNeuronType additionalInputNeuronType = NetworkHelper.AdditionalInputNeuronType.And,
            params Neuron[] additionalInputs
        );
    }
}
