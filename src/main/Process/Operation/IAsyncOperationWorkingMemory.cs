using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public interface IAsyncOperationWorkingMemory : IWorkingMemory
    {
        IEnumerable<INeuronChunk> GetCurrent();
    }
}
