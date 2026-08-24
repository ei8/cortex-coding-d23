using System;

namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class FiniteProcessBase
    <
        TProcess,
        TWorkingMemory,
        TCompletion
    >
    (
        TWorkingMemory workingMemory,
        TCompletion completionCallback
    ) :
        ProcessBase<TWorkingMemory>(workingMemory)
        where TProcess : IProcess<TWorkingMemory>
        where TWorkingMemory : IWorkingMemory
        where TCompletion : Delegate
    {
        protected TCompletion completionCallback = completionCallback;
    }
}
