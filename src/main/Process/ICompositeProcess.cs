namespace ei8.Cortex.Coding.d23.Process
{
    public interface ICompositeProcess : IProcess
    {
    }

    public interface ICompositeProcess<TProcess> :
        ICompositeProcess
        where TProcess : IProcess
    {
        TProcess Process { get; }
    }
}
