using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class LogicGateBase<TInput, TOutput>(
        FunctionalCircuitParameter<TInput, TOutput> parameters,
        IEnumerable<ReadOnlyNetwork> networks,
        VariableInfo? variableInfo
    ) : FunctionalCircuitBase<TInput, TOutput>(
        parameters,
        networks,
        variableInfo
    )
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
    }
}