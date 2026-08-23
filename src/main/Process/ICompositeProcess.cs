namespace ei8.Cortex.Coding.d23.Process
{
    public interface ICompositeProcess : IProcess
    {
    }

    public interface ICompositeProcess<TProcess1> :
        ICompositeProcess
        where TProcess1 : IProcess
    {
        TProcess1 Process1 { get; }
    }
}
