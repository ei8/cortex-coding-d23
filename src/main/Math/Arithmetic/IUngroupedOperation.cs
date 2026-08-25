namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public interface IUngroupedOperation
    <
        T,
        TParam,
        TInterneuron
    > :
        IOperation
        <
            T,
            TParam,
            TInterneuron
        >
        where T : ICircuit<TParam, TInterneuron>
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        static abstract TParam GetDefaultParameters(
            int exponent
        );

        static abstract TInterneuron CreateInterneurons(
            TParam parameters,
            VariableInfo variableInfo
        );
    }
}
