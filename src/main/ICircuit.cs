namespace ei8.Cortex.Coding.d23
{
    public interface ICircuit<TParam, TInterneuron> : IneurUL 
        where TParam : ICircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        TParam Parameters { get; }

        TInterneuron Interneurons { get; }
    }
}
