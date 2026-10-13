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

    public interface IChunk<T1> : IChunk
        where T1 : IChunk?
    {
        T1 Content1 { get; }
    }

    public interface IChunk<T1, T2> : IChunk<T1>
        where T1 : IChunk?
        where T2 : IChunk?
    {
        T2 Content2 { get; }
    }

    public interface IChunk<T1, T2, T3> : IChunk<T1, T2>
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
    {
        T3 Content3 { get; }
    }

    public interface IChunk<T1, T2, T3, T4> : IChunk<T1, T2, T3>
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
    {
        T4 Content4 { get; }
    }

    public interface IChunk<T1, T2, T3, T4, T5> : IChunk<T1, T2, T3, T4>
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
        where T5 : IChunk?
    {
        T5 Content5 { get; }
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

    public interface IListChunk<T> : IEnumerableChunkCore<IList<T>, T>
        where T : IChunk
    {
    }

    public interface INestedListChunk<TList, T> : IChunk
        where TList : IListChunk<T>
        where T : IChunk
    {
        IList<TList> Lists { get; }
    }
}
