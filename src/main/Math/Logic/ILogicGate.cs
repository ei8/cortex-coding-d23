using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public interface ILogicGate<T, TInput, TOutput, TInterneuron> : ICircuit<FunctionalCircuitParameter<TInput, TOutput>, TInterneuron>
        where T : ICircuit<FunctionalCircuitParameter<TInput, TOutput>, TInterneuron>
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
        where TInterneuron : ICircuitInterneuronSet
    {
        static abstract IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output);

        static abstract IEnumerable<string> GetInterneuronTags(
            VariableInfo variableInfo,
            InterneuronTagInfo? interneuronTagInfo = null
        );

        static abstract T Create(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            TInterneuron interneurons,
            VariableInfo? variableInfo
        );

        static abstract IEnumerable<ReadOnlyNetwork> LinkInputNeurons(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            NetworkHelper.AdditionalInputNeuronType additionalInputNeuronType = NetworkHelper.AdditionalInputNeuronType.And,
            params Neuron[] additionalInputs
        );
    }
}
