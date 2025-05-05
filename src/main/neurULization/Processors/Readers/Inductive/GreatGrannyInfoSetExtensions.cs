using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    internal static class GreatGrannyInfoSetExtensions
    {
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

        internal static IGreatGrannyInfoSuperset<TResult> Append<TResult>(
            this IGreatGrannyInfoSuperset<TResult> value,
            IGreatGrannyInfoSet<TResult> element
        )
            where TResult : IGranny
        {
            value.Add(element);
            return value;
        }
    }
}
