namespace ei8.Cortex.Coding.d23
{
    public interface IAugmentedCircuit<T> :
        ICircuit
        where T : IAugmentation
    {
        T Augmentation { get; }
    }
}
