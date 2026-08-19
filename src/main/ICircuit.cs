namespace ei8.Cortex.Coding.d23
{
    public interface ICircuit : IneurUL
    {
    }

    public interface ICircuit<TParam, TInterneuron> : ICircuit
        where TParam : ICircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        TParam Parameters { get; }

        TInterneuron Interneurons { get; }
    }
}
