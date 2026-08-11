using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public interface IWorkingMemory
    {
    }

    public interface IWorkingMemory<T> : IReadOnlyDictionary<T, IChunk>, IWorkingMemory
        where T : notnull
    {
    }
}
