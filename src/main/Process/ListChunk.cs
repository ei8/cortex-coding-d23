using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class ListChunk<T>(IList<T> items) : IListChunk<T>
        where T : IChunk
    {
        public ListChunk() : this([]) { }

        public IList<T> Items { get; } = items;
    }
}
