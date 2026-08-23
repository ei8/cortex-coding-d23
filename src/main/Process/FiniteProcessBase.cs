using System;

namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class FiniteProcessBase
    <
        TProcess, 
        TWorkingMemory
    >
    (
        TWorkingMemory workingMemory,
        Action<TProcess> completionCallback
    ) :
        ProcessBase<TWorkingMemory>(workingMemory)
        where TProcess: IProcess<TWorkingMemory>
        where TWorkingMemory : IWorkingMemory
    {
        protected Action<TProcess> completionCallback = completionCallback;
    }
}
