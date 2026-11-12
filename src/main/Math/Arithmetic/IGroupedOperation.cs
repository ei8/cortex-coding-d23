namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public interface IGroupedOperation
    {
    }

    public interface IGroupedOperation
    <
        TParam,
        TInterneuron
    > :
        IGroupedOperation,
        IOperation
        <
            TParam,
            TInterneuron
        >
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
    }

    public interface IGroupedOperationStatic
    <
        T, 
        TParam, 
        TInterneuron
    > : 
        IGroupedOperation
        <
            TParam,
            TInterneuron
        >
        where T : ICircuit<TParam, TInterneuron>
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        static abstract TParam CreateDefaultParameters(
            BinaryNeuronParameter precedingValue,
            int exponent
        );
    }
}
