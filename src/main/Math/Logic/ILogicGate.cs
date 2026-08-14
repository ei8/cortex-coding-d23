using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public interface ILogicGate<T, TInput, TOutput> : ICircuit<FunctionalCircuitParameter<TInput, TOutput>>
        where T : ICircuit<FunctionalCircuitParameter<TInput, TOutput>>
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
        static abstract IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output);

        static abstract IEnumerable<string> GetInterneuronTags(
            VariableInfo variableInfo,
            InterneuronTagInfo? interneuronTagInfo = null
        );

        static abstract T Create(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        );

        static abstract IEnumerable<ReadOnlyNetwork> LinkInputNeurons(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            params Neuron[] additionalInputs
        );
    }
}
