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
}
