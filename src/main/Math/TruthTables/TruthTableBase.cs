namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public abstract class TruthTableBase
    <
        TParam,
        TInterneuron
    >
    (
        TParam parameters,
        TInterneuron interneuron,
        VariableInfo? variableInfo
    ) :
        FunctionalCircuitBase
        <
            TParam,
            TInterneuron
        >
        (
            parameters,
            interneuron,
            variableInfo
        )
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
    }
}
