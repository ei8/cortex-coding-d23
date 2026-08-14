namespace ei8.Cortex.Coding.d23
{
    public interface ICircuit<TParam> : IneurUL 
        where TParam : CircuitParameterBase
    {
        TParam Parameters { get; }
    }
}
