namespace ei8.Cortex.Coding.d23.Collections
{
    public interface IBiphasicAdjacent
    <
        T, 
        TParam, 
        TInterneuron
    > : 
        IAdjacent
        <
            T, 
            TParam, 
            TInterneuron
        >
        where T : ICircuit<TParam, TInterneuron>
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        static abstract TInterneuron CreateInterneurons
        (
            TParam parameters,
            VariableInfo variableInfo,
            TInterneuron? precedingInterneurons = default,
            bool linkPhaseInterneurons = false,
            params Neuron[] additionalInputs
        );
    }
}
