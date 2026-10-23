using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class ConcurrentListChunk<T> : IConcurrentListChunk<T>
        where T : IChunk
    {
        private ConcurrentDictionary<int, T> innerDictionary;

        public ConcurrentListChunk(IEnumerable<T> items)
        {
            this.innerDictionary = new ConcurrentDictionary<int, T>();
            foreach (var item in items)
                this.Add(item);
        }

        public ConcurrentListChunk() : this([]) { }

        public IEnumerable<T> Items => this.innerDictionary.Values;

        public int Count => this.innerDictionary.Count;

        public void Add(T item)
        {
            this.innerDictionary.TryAdd(this.innerDictionary.Count, item);
        }

        public void Clear()
        {
            this.innerDictionary.Clear();
        }

        public T ItemAt(int index)
        {
            if (this.innerDictionary.TryGetValue(index, out var result))
                return result;
            else
                throw new IndexOutOfRangeException();
        }
    }
}