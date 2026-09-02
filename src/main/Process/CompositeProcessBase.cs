namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class CompositeProcessBase
    <
        TWorkingMemory, 
        TProcess1
    >
    (
        TWorkingMemory workingMemory
    ) :
        ProcessBase<TWorkingMemory>
        (
            workingMemory
        ),
        ICompositeProcess<TProcess1>
        where TProcess1 : IProcess
        where TWorkingMemory : IWorkingMemory
    {
        public TProcess1? Process1 { get; protected set; }
    }
}
