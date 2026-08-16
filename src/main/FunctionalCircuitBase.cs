using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public abstract class FunctionalCircuitBase<TInput, TOutput>
    (
        FunctionalCircuitParameter<TInput, TOutput> parameters,
        IEnumerable<ReadOnlyNetwork> networks,
        VariableInfo? variableInfo
    ) : CircuitBase<FunctionalCircuitParameter<TInput, TOutput>>
    (
        parameters,
        networks,
        variableInfo
    )
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
    }
}
