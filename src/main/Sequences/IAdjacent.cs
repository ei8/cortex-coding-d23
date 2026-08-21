namespace ei8.Cortex.Coding.d23.Sequences
{
    public interface IAdjacent
    <
        T, 
        TParam, 
        TInterneuron
    > : 
        ICircuit
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
