namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public interface IUngroupedOperation
    {
    }

    public interface IUngroupedOperation
    <
        TParam,
        TInterneuron
    > :
        IUngroupedOperation,
        IOperation
        <
            TParam,
            TInterneuron
        >
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
    }

    public interface IUngroupedOperationStatic
    <
        T,
        TParam,
        TInterneuron
    > :
        IUngroupedOperation
        <
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
