using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    /// <summary>
    /// Chunking is the recoding of smaller units of information into larger, familiar units. Chunking is often assumed to help bypassing the limited capacity of working memory (WM).
    /// Thalmann M, Souza AS, Oberauer K. How does chunking help working memory? J Exp Psychol Learn Mem Cogn. 2019 Jan;45(1):37-55. doi: 10.1037/xlm0000578. Epub 2018 Apr 26. PMID: 29698045.
    /// </summary>
    public interface IChunk
    {
    }

    public interface INeuronChunk : IChunk 
    {
        Neuron Neuron { get; }
    }

    public interface INamedChunk : IChunk
    {
        string Name { get; }
    }

    public interface IEnumerableChunkCore<T, TItem> : IChunk
        where T : IEnumerable<TItem>
        where TItem : IChunk
    {
        T Items { get; }
    }

    public interface IEnumerableChunk<T> : IEnumerableChunkCore<IEnumerable<T>, T>
        where T : IChunk
    {
    }

    public interface IConcurrentListChunk<T> : IEnumerableChunkCore<IEnumerable<T>, T>
        where T : IChunk
    {
        void Add(T item);

        void Clear();

        T ItemAt(int index);

        int Count { get; }
    }

    public interface IListChunk<T> : IEnumerableChunkCore<IList<T>, T>
        where T : IChunk
    {
    }

    public interface INestedListChunk<TList, TChunk> : IChunk
        where TList : IListChunk<TChunk>
        where TChunk : IChunk
    {
        IList<TList> Lists { get; }
    }

    public interface INestedNeuronListChunk<TList> : INestedListChunk<TList, INeuronChunk>
        where TList : IListChunk<INeuronChunk>
    {
    }
}
