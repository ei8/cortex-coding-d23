using System;

namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class FiniteCompositeProcessBase
    <
        TWorkingMemory, 
        TProcess1,
        TCompletion
    >
    (
        TWorkingMemory workingMemory, 
        TCompletion completionCallback
    ) :
        CompositeProcessBase<TWorkingMemory, TProcess1>(workingMemory)
        where TWorkingMemory : IWorkingMemory
        where TProcess1 : IProcess
        where TCompletion : Delegate
    {
        protected TCompletion completionCallback = completionCallback;
    }

    public abstract class FiniteCompositeProcessBase
    <
        TWorkingMemory,
        TProcess1,
        TProcess2,
        TCompletion
    >
    (
        TWorkingMemory workingMemory,
        TCompletion completionCallback
    ) :
        CompositeProcessBase
        <
            TWorkingMemory, 
            TProcess1,
            TProcess2
        >
        (
            workingMemory
        )
        where TWorkingMemory : IWorkingMemory
        where TProcess1 : IProcess
        where TProcess2 : IProcess
        where TCompletion : Delegate
    {
        protected TCompletion completionCallback = completionCallback;
    }

    public abstract class FiniteCompositeProcessBase
    <
        TWorkingMemory,
        TProcess1,
        TProcess2,
        TProcess3,
        TCompletion
    >
    (
        TWorkingMemory workingMemory,
        TCompletion completionCallback
    ) :
        CompositeProcessBase
        <
            TWorkingMemory,
            TProcess1,
            TProcess2,
            TProcess3
        >
        (
            workingMemory
        )
        where TWorkingMemory : IWorkingMemory
        where TProcess1 : IProcess
        where TProcess2 : IProcess
        where TProcess3 : IProcess
        where TCompletion : Delegate
    {
        protected TCompletion completionCallback = completionCallback;
    }
}
