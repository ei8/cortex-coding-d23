using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class LogicGateBase<TInput, TOutput, TInterneuron>
    (
        FunctionalCircuitParameter<TInput, TOutput> parameters,
        TInterneuron interneuron,
        VariableInfo? variableInfo
    ) : 
        FunctionalCircuitBase<TInput, TOutput, TInterneuron>(
            parameters,
            interneuron,
            variableInfo
        )
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
        where TInterneuron : ICircuitInterneuronSet
    {
    }
}