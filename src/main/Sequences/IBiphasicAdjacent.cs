namespace ei8.Cortex.Coding.d23.Sequences
{
    public interface IBiphasicAdjacent
    {
    }

    public interface IBiphasicAdjacent
    <
        TParam,
        TInterneuron
    > :
        IBiphasicAdjacent,
        IAdjacent
        <
            TParam,
            TInterneuron
        >
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
    }
    
    public interface IBiphasicAdjacentStatic
    <
        T, 
        TParam, 
        TInterneuron
    > : 
        IBiphasicAdjacent
        <
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
            float inputStrength = 0.5f,
            TInterneuron? precedingInterneurons = default,
            float interPhaseStrength = 1f,
            params NeuronInfo[] additionalInputs
        );
    }
}
