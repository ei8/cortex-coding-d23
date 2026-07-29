using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23
{
    public class FunctionalParameter<T> : ProceduralParameter<T>
        where T : NeuronInfoBase
    {
        public FunctionalParameter() : this(Enumerable.Empty<T?>(), Enumerable.Empty<T?>())
        {
        }

        public FunctionalParameter(IEnumerable<T?> inputs, IEnumerable<T?> outputs) : base(inputs)
        {
            this.network.AddReplaceItems(
                [
                    ..(this.Outputs = outputs).WhereNotNull()
                ]
            );
        }

        public IEnumerable<T?> Outputs { get; }
    }
}
