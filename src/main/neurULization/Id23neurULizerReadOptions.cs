using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface Id23neurULizerReadOptions : IneurULizerReadOptions, Id23neurULizerOptions
    {
        Processors.Readers.Inductive.IInstanceProcessor InstanceProcessor { get; }

        IInstantiatesClass InstantiatesClass { get; }
    }
}
