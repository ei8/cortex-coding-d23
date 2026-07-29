using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23
{
    public class FunctionalParameterInfo<T> : ProceduralParameterInfo<T>
        where T : NeuronInfoBase
    {
        public FunctionalParameterInfo() : this(Enumerable.Empty<T?>(), Enumerable.Empty<T?>())
        {
        }

        public FunctionalParameterInfo(IEnumerable<T?> inputs, IEnumerable<T?> outputs) : base(inputs)
        {
            this.Outputs = outputs;

            this.network.AddReplaceItems(
                [
                    ..this.Outputs.WhereNotNull()
                ]
            );
        }

        public IEnumerable<T?> Outputs { get; }
    }
}
