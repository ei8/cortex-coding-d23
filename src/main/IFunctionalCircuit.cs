namespace ei8.Cortex.Coding.d23
{
    public interface IFunctionalCircuit
    <
        TParam, 
        TInterneuron
    > : 
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
