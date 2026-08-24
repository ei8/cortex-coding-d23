using System;

namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class FiniteCompositeProcessBase
    <
        TProcess, 
        TWorkingMemory, 
        TProcess1,
        TCompletion
    >
    (
        TWorkingMemory workingMemory, 
        TCompletion completionCallback
    ) :
        CompositeProcessBase<TWorkingMemory, TProcess1>(workingMemory)
        where TProcess : IProcess<TWorkingMemory>
        where TWorkingMemory : IWorkingMemory
        where TProcess1 : IProcess
        where TCompletion : Delegate
    {
        protected TCompletion completionCallback = completionCallback;
    }
}
