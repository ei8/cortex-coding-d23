using System;

namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class FiniteCompositeProcessBase
    <
        TProcess, 
        TWorkingMemory, 
        TProcess1,
        TResult
    >
    (
        TWorkingMemory workingMemory, 
        Action<TProcess, IProcess?, TResult> completionCallback
    ) :
        CompositeProcessBase<TWorkingMemory, TProcess1>(workingMemory)
        where TProcess : IProcess<TWorkingMemory>
        where TWorkingMemory : IWorkingMemory
        where TProcess1 : IProcess
    {
        protected Action<TProcess, IProcess?, TResult> completionCallback = completionCallback;
    }
}
