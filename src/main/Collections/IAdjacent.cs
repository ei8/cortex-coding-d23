namespace ei8.Cortex.Coding.d23.Collections
{
    public interface IAdjacent<T, TInput, TOutput, TInterneuron> : ICircuit<FunctionalCircuitParameter<TInput, TOutput>, TInterneuron>
        where T : ICircuit<FunctionalCircuitParameter<TInput, TOutput>, TInterneuron>
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
        where TInterneuron : ICircuitInterneuronSet
    {
        static abstract TInterneuron CreateInterneurons(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            VariableInfo variableInfo,
            TInterneuron? precedingInterneuronNetwork = default,
            params Neuron[] additionalInputs
        );

        static abstract T Create(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            TInterneuron interneurons,
            VariableInfo? variableInfo
        );
    }
}
