namespace ei8.Cortex.Coding.d23.Process
{
    public interface ICompositeProcess : IProcess
    {
    }

    public interface ICompositeProcess<TProcess1> :
        ICompositeProcess
        where TProcess1 : IProcess
    {
        TProcess1? Process1 { get; }
    }

    public interface ICompositeProcess
    <
        TProcess1, 
        TProcess2
    > :
        ICompositeProcess
        where TProcess1 : IProcess
        where TProcess2 : IProcess
    {
        TProcess2? Process2 { get; }
    }

    public interface ICompositeProcess
    <
        TProcess1, 
        TProcess2, 
        TProcess3
    > :
        ICompositeProcess
        where TProcess1 : IProcess
        where TProcess2 : IProcess
        where TProcess3 : IProcess
    {
        TProcess3? Process3 { get; }
    }
}
