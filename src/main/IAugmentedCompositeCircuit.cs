namespace ei8.Cortex.Coding.d23
{
    public interface IAugmentedCompositeCircuit<T> : 
        ICompositeCircuit, 
        IAugmentedCircuit<T>
        where T : IAugmentation
    {
    }

    public interface IAugmentedCompositeCircuit<T, TAug, T1, T2> :
        ICompositeCircuit<T1, T2>,
        IAugmentedCircuit<TAug>
        where T : ICompositeCircuit<T1, T2>
        where TAug : IAugmentation
        where T1 : ICircuit
        where T2 : ICircuit
    {
        static abstract T Create(TAug augmentation, T1 circuit1, T2 circuit2, VariableInfo? variableInfo);
    }
}
