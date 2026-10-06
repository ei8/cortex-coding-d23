namespace ei8.Cortex.Coding.d23
{
    public interface IFunctionalCircuitParameter : ICircuitParameter
    {
    }

    public interface IFunctionalCircuitParameter<TInput> : IFunctionalCircuitParameter, ICircuitParameter<TInput>
        where TInput : IInputCircuitParameterSubset
    {
    }

    public interface IFunctionalCircuitParameter<TInput, TOutput> : IFunctionalCircuitParameter<TInput>
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
        TOutput Outputs { get; }
    }
}
