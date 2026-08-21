using System;

namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class CompositeProcessBase<TWorkingMemory, TProcess>(TWorkingMemory workingMemory, TProcess process) :
        ProcessBase<TWorkingMemory>
        (
            workingMemory
        ),
        ICompositeProcess<TProcess>
        where TProcess : IProcess
        where TWorkingMemory : IWorkingMemory
    {
        public TProcess Process => process;
    }
}
