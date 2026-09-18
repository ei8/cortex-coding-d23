namespace ei8.Cortex.Coding.d23.Sequences
{
    public interface IAdjacent
    {
    }

    public interface IAdjacent
    <
        TParam,
        TInterneuron
    > :
        IAdjacent,
        ICircuit
        <
            TParam,
            TInterneuron
        >
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
    }

    public interface IAdjacentStatic
    <
        T, 
        TParam, 
        TInterneuron
    > : 
        IAdjacent
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
            params NeuronInfo[] additionalInputNeuronInfos
        );

        static abstract T Create
        (
            TParam parameters,
            TInterneuron interneurons,
            VariableInfo? variableInfo
        );
    }
}
