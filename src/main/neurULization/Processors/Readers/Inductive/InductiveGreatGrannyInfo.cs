using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    // TODO:1 Rename Aggregate to LesserGranny, and TGranny, TResult to TGreatGranny where applicable
    public interface IInductiveGreatGrannyInfo<TAggregate> : 
        IGreatGrannyInfo<TAggregate>
    {
        IEnumerable<Neuron> Neurons { get; }
    }
}
