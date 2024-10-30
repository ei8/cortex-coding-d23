using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class GreatGrannyInfoSet<TResult> : IGreatGrannyInfoSet<TResult>
        where TResult : IGranny
    {
        public GreatGrannyInfoSet(IEnumerable<IGreatGrannyInfo<TResult>> items)
        {
            this.Items = items;
        }

        public IEnumerable<IGreatGrannyInfo<TResult>> Items { get; }
    }
}
