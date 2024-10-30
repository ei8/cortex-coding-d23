using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class GreatGrannyInfoSet<TResult> : IGreatGrannyInfoSet<TResult>
        where TResult : IGranny
    {
        public GreatGrannyInfoSet(IEnumerable<IGreatGrannyInfo<TResult>> items, IGreatGrannyProcess<TResult> target)
        {
            this.Items = items;
            this.Target = target;
        }

        public IEnumerable<IGreatGrannyInfo<TResult>> Items { get; }

        public IGreatGrannyProcess<TResult> Target { get; }
    }
}
