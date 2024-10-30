using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public interface IGreatGrannyInfoSet<TResult>
        where TResult : IGranny
    {
        IEnumerable<IGreatGrannyInfo<TResult>> Items { get; }

        IGreatGrannyProcess<TResult> Target { get; }
    }
}
