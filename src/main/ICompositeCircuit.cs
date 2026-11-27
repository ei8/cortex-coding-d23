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

    public interface ICompositeCircuitStatic<T, T1, T2> :
        ICompositeCircuit<T1, T2>
        where T : ICompositeCircuitStatic<T, T1, T2>
        where T1 : ICircuit
        where T2 : ICircuit
    {
        static abstract T Create(T1 circuit1, T2 circuit2, VariableInfo? variableInfo);
    }

    public interface ICompositeCircuit<T1, T2, T3> :
        ICompositeCircuit
        where T1 : ICircuit
        where T2 : ICircuit
        where T3 : ICircuit
    {
        T1 Circuit1 { get; }

        T2 Circuit2 { get; }

        T3 Circuit3 { get; }
    }

    public interface ICompositeCircuitStatic<T, T1, T2, T3> :
        ICompositeCircuit<T1, T2, T3>
        where T : ICompositeCircuitStatic<T, T1, T2, T3>
        where T1 : ICircuit
        where T2 : ICircuit
        where T3 : ICircuit
    {
        static abstract T Create(T1 circuit1, T2 circuit2, T3 circuit3, VariableInfo? variableInfo);
    }

    public interface ICompositeCircuit<T1, T2, T3, T4> :
        ICompositeCircuit
        where T1 : ICircuit
        where T2 : ICircuit
        where T3 : ICircuit
        where T4 : ICircuit
    {
        T1 Circuit1 { get; }

        T2 Circuit2 { get; }

        T3 Circuit3 { get; }

        T4 Circuit4 { get; }
    }

    public interface ICompositeCircuitStatic<T, T1, T2, T3, T4> :
        ICompositeCircuit<T1, T2, T3, T4>
        where T : ICompositeCircuitStatic<T, T1, T2, T3, T4>
        where T1 : ICircuit
        where T2 : ICircuit
        where T3 : ICircuit
        where T4 : ICircuit
    {
        static abstract T Create(T1 circuit1, T2 circuit2, T3 circuit3, T4 circuit4, VariableInfo? variableInfo);
    }
}
