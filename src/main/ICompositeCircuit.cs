namespace ei8.Cortex.Coding.d23
{
    public interface ICompositeCircuit : 
        ICircuit
    {
    }

    public interface ICompositeCircuit<T, T1, T2> : 
        ICompositeCircuit
        where T : ICompositeCircuit<T, T1, T2>
        where T1 : ICircuit
        where T2 : ICircuit
    {
        T1 Circuit1 { get; }

        T2 Circuit2 { get; }
    }

    public interface ICreatableCompositeCircuit<T, T1, T2> :
        ICompositeCircuit<T, T1, T2>
        where T : ICompositeCircuit<T, T1, T2>
        where T1 : ICircuit
        where T2 : ICircuit
    {
        static abstract T Create(T1 circuit1, T2 circuit2, VariableInfo? variableInfo);
    }
}
