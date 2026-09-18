namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public interface IOperation
    {
    }

    public interface IOperation
    <
        TParam,
        TInterneuron
    > :
        IOperation,
        ICircuit
        <
            TParam,
            TInterneuron
        >
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
    }
    
    public interface IOperationStatic
    <
        T,
        TParam,
        TInterneuron
    > :
        IOperation
        <
            TParam,
            TInterneuron
        >
        where T : ICircuit<TParam, TInterneuron>
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        static abstract T Create(
            TParam parameters,
            TInterneuron interneurons,
            VariableInfo? variableInfo
        );
    }
}
