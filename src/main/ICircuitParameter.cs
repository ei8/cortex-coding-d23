namespace ei8.Cortex.Coding.d23
{
    public interface ICircuitParameter : IneurUL
    {
    }

    public interface ICircuitParameter<TInput> : ICircuitParameter
        where TInput : IInputCircuitParameterSubset
    {
        TInput Inputs { get; }
    }
}
