namespace ei8.Cortex.Coding.d23
{
    public interface ICreatableCompositeCircuit<T, T1, T2> :
        ICompositeCircuit<T1, T2>
        where T : ICreatableCompositeCircuit<T, T1, T2>
        where T1 : ICircuit
        where T2 : ICircuit
    {
        static abstract T Create(T1 circuit1, T2 circuit2, VariableInfo? variableInfo);
    }
}
