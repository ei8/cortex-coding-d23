using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public class ProceduralParameter<T> : ParameterBase<T>
        where T : NeuronParameterBase
    {
        public ProceduralParameter(IEnumerable<T?> inputs) : base()
        {
            this.network.AddReplaceItems(
                [
                    ..(this.Inputs = inputs).WhereNotNull()
                ]
            );
        }

        public IEnumerable<T?> Inputs { get; }
    }
}
