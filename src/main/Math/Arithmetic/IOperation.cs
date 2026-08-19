namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public interface IOperation
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
        static abstract TParam GetDefaultParameters(
            BinaryNeuronParameter? precedingValue,
            int exponent
        );

        static abstract TInterneuron CreateInterneurons(
            TParam parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        );

        static abstract T Create(
            TParam parameters,
            TInterneuron interneurons,
            VariableInfo? variableInfo
        );
    }
}
