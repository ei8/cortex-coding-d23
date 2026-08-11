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

    public interface IContentChunk<T>
        where T : IEnumerable<Neuron>
    {
        T Contents { get; }
    }

    public interface IKeyedChunk<T> : IChunk
    {
        T Key { get; }
    }

    public interface IReadableChunk<T> : IContentChunk<IEnumerable<Neuron>>, IKeyedChunk<T>
    {
    }

    public interface IWriteableChunk<T> : IContentChunk<IList<Neuron>>, IKeyedChunk<T>
    {
    }
}
