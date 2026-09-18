using System;

namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class FiniteProcessBase
    <
        TWorkingMemory,
        TCompletion
    >
    (
        TWorkingMemory workingMemory,
        TCompletion completionCallback
    ) :
        ProcessBase<TWorkingMemory>(workingMemory)
        where TWorkingMemory : IWorkingMemory
        where TCompletion : Delegate
    {
        protected TCompletion completionCallback = completionCallback;
    }
}
