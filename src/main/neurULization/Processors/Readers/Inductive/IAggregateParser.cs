using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public interface IAggregateParser
    {
        bool TryParse<TConcrete, TResult>(
            Neuron granny,
            IGreatGrannyInfoSuperset<TResult> greatGrannyCandidates,
            Network network,
            out TResult result
        )
            where TConcrete : TResult, new()
            where TResult : IGranny;
    }
}
