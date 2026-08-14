namespace ei8.Cortex.Coding.d23.Collections
{
    public interface IAdjacent<T, TInput, TOutput> : ICircuit<FunctionalCircuitParameter<TInput, TOutput>>
        where T : ICircuit<FunctionalCircuitParameter<TInput, TOutput>>
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
        static abstract ReadOnlyNetwork CreateInterneuronNetwork(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            VariableInfo variableInfo
        );

        static abstract ReadOnlyNetwork LinkInputNeurons(
            ReadOnlyNetwork interneuronNetwork,
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            ReadOnlyNetwork? precedingInterneuronNetwork = null,
            params Neuron[] additionalInputs
        );

        static abstract T Create(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            ReadOnlyNetwork interneuronNetwork,
            ReadOnlyNetwork linkedInputNeurons,
            VariableInfo? variableInfo
        );
    }
}
