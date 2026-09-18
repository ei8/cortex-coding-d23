using System;

namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class FiniteCompositeProcessBase
    <
        TWorkingMemory, 
        TProcess,
        TCompletion
    >
    (
        TWorkingMemory workingMemory, 
        TCompletion completionCallback
    ) :
        CompositeProcessBase<TWorkingMemory, TProcess>(workingMemory)
        where TWorkingMemory : IWorkingMemory
        where TProcess : IProcess
        where TCompletion : Delegate
    {
        protected TCompletion completionCallback = completionCallback;
    }
}
