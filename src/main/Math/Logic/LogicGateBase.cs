namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class LogicGateBase<TParam, TInterneuron>
    (
        TParam parameters,
        TInterneuron interneuron,
        VariableInfo? variableInfo
    ) : 
        FunctionalCircuitBase<TParam, TInterneuron>(
            parameters,
            interneuron,
            variableInfo
        )
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
    }
}