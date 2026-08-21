namespace ei8.Cortex.Coding.d23
{
    public interface ICompositeCircuit : 
        ICircuit
    {
    }

    public interface ICompositeCircuit<T1, T2> : 
        ICompositeCircuit
        where T1 : ICircuit
        where T2 : ICircuit
    {
        T1 Circuit1 { get; }

        T2 Circuit2 { get; }
    }
}
