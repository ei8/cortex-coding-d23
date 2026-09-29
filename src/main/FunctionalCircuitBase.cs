namespace ei8.Cortex.Coding.d23
{
    public abstract class FunctionalCircuitBase<TParam, TInterneuron>
    (
        TParam parameters,
        TInterneuron interneurons,
        VariableInfo? variableInfo
    ) : 
        CircuitBase<TParam, TInterneuron>
        (
            parameters,
            interneurons,
            variableInfo
        ),
        IFunctionalCircuit
        <
            TParam,
            TInterneuron
        >
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
    }
}
