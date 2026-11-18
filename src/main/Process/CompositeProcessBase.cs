namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class CompositeProcessBase
    <
        TWorkingMemory, 
        TProcess
    >
    (
        TWorkingMemory workingMemory
    ) :
        ProcessBase<TWorkingMemory>
        (
            workingMemory
        ),
        ICompositeProcess<TProcess>
        where TProcess : IProcess
        where TWorkingMemory : IWorkingMemory
    {
        public TProcess? Process1 { get; protected set; }
    }

    public abstract class CompositeProcessBase
    <
        TWorkingMemory,
        TProcess1,
        TProcess2
    >
    (
        TWorkingMemory workingMemory
    ) :
        ProcessBase<TWorkingMemory>
        (
            workingMemory
        ),
        ICompositeProcess
        <
            TProcess1,
            TProcess2
        >
        where TProcess1 : IProcess
        where TProcess2 : IProcess
        where TWorkingMemory : IWorkingMemory
    {
        public TProcess1? Process1 { get; protected set; }
        public TProcess2? Process2 { get; protected set; }
    }

    public abstract class CompositeProcessBase
    <
        TWorkingMemory,
        TProcess1,
        TProcess2,
        TProcess3
    >
    (
        TWorkingMemory workingMemory
    ) :
        ProcessBase<TWorkingMemory>
        (
            workingMemory
        ),
        ICompositeProcess
        <
            TProcess1,
            TProcess2,
            TProcess3
        >
        where TProcess1 : IProcess
        where TProcess2 : IProcess
        where TProcess3 : IProcess
        where TWorkingMemory : IWorkingMemory
    {
        public TProcess1? Process1 { get; protected set; }
        public TProcess2? Process2 { get; protected set; }
        public TProcess3? Process3 { get; protected set; }
    }
}
