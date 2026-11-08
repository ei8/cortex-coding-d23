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
}
