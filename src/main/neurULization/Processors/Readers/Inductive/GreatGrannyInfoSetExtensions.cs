using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    internal static class GreatGrannyInfoSetExtensions
    {
        internal static IGreatGrannyInfoSuperset<TResult> AsSuperset<TResult>(this IGreatGrannyInfoSet<TResult> value)
            where TResult : IGranny
        => GreatGrannyInfoSuperset<TResult>.Create( new IGreatGrannyInfoSet<TResult>[] { value });

        internal static IGreatGrannyInfoSuperset<TResult> Concat<TResult>(
            this IGreatGrannyInfoSuperset<TResult> first,
            IGreatGrannyInfoSuperset<TResult> second,
            bool excludeEmptySets = true
        )
            where TResult : IGranny
        {
            var result = GreatGrannyInfoSuperset<TResult>.Create(first.Items, excludeEmptySets);
            result.AddRange(second.Items); 
            return result;
        }
    }
}
