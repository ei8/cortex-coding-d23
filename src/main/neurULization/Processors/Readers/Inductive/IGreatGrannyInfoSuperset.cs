using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public interface IGreatGrannyInfoSuperset<TResult>
        where TResult : IGranny
    {
        void AddRange(IEnumerable<IGreatGrannyInfoSet<TResult>> values);

        void Add(IGreatGrannyInfoSet<TResult> value);

        IEnumerable<IGreatGrannyInfoSet<TResult>> Items { get; }

        int Count { get; }
    }
}
