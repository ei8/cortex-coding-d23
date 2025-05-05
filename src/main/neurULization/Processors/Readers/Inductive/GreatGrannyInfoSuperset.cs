using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class GreatGrannyInfoSuperset<TResult> : IGreatGrannyInfoSuperset<TResult>
        where TResult : IGranny
    {
        private readonly IList<IGreatGrannyInfoSet<TResult>> list;
        private readonly bool excludeEmptySets;

        public static GreatGrannyInfoSuperset<TResult> Create(
            IGreatGrannyInfoSet<TResult> value,
            bool excludeEmptySets = true
        ) => GreatGrannyInfoSuperset<TResult>.Create(
            new List<IGreatGrannyInfoSet<TResult>>() { value }, 
            excludeEmptySets
        );

        public static GreatGrannyInfoSuperset<TResult> Create(
            IEnumerable<IGreatGrannyInfoSet<TResult>> values, 
            bool excludeEmptySets = true
        )
        {
            var result = new GreatGrannyInfoSuperset<TResult>(excludeEmptySets);
            result.AddRange(values);
            return result;
        }

        private GreatGrannyInfoSuperset(bool excludeEmptySets)
        {
            this.list = new List<IGreatGrannyInfoSet<TResult>>();
            this.excludeEmptySets = excludeEmptySets;
        }

        public void AddRange(IEnumerable<IGreatGrannyInfoSet<TResult>> values) =>
            values.ToList().ForEach(v => this.Add(v));

        public void Add(IGreatGrannyInfoSet<TResult> value)
        {
            if (!this.excludeEmptySets || value.Items.Count() > 0)
                this.list.Add(value);
        }

        public IEnumerable<IGreatGrannyInfoSet<TResult>> Items => this.list;

        public int Count => this.list.Count;
    }
}
