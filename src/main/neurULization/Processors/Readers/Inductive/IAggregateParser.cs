using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public interface IAggregateParser
    {
        bool TryParse<TConcrete, TResult>(
            Neuron granny,
            IEnumerable<IGreatGrannyInfo<TResult>> greatGrannyCandidates,
            IEnumerable<IGreatGrannyProcess<TResult>> targets,
            Ensemble ensemble,
            int expectedGreatGrannyCount,
            out TResult result
        )
            where TConcrete : TResult, new()
            where TResult : IGranny;
    }
}
