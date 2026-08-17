namespace ei8.Cortex.Coding.d23.Collections
{
    public interface IAdjacent<T, TParam, TInterneuron> : ICircuit<TParam, TInterneuron>
        where T : ICircuit<TParam, TInterneuron>
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        static abstract TInterneuron CreateInterneurons(
            TParam parameters,
            VariableInfo variableInfo,
            TInterneuron? precedingInterneurons = default,
            params Neuron[] additionalInputs
        );

        static abstract T Create(
            TParam parameters,
            TInterneuron interneurons,
            VariableInfo? variableInfo
        );
    }
}
