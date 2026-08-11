using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class WorkingMemory<T>: Dictionary<T, IChunk>, IWorkingMemory<T>
        where T : notnull
    {
        public WorkingMemory(params IKeyedChunk<T>[] initial) : base()
        {
            foreach (var c in initial)
                this.Add(c.Key, c);
        }
    }
}
