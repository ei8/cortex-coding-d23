using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public interface ISyncOperationWorkingMemory : IWorkingMemory
    {
        IEnumerable<INeuronChunk> GetCurrent(int digitIndex);
    }
}
