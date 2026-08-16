namespace ei8.Cortex.Coding.d23
{
    // TODO: can we just specify TFunctionalParam instead of TInput + TOutput
    public abstract class FunctionalCircuitBase<TInput, TOutput, TInterneuron>
    (
        FunctionalCircuitParameter<TInput, TOutput> parameters,
        TInterneuron interneurons,
        VariableInfo? variableInfo
    ) : 
        CircuitBase<FunctionalCircuitParameter<TInput, TOutput>, TInterneuron>
        (
            parameters,
            interneurons,
            variableInfo
        )
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
        where TInterneuron : ICircuitInterneuronSet
    {
    }
}
