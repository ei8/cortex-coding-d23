using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public class ProceduralParameterInfo<T> : IneurUL
        where T : NeuronInfoBase
    {
        protected Network network;

        public ProceduralParameterInfo(IEnumerable<T?> inputs)
        {
            this.Inputs = inputs;

            this.network = new();
            this.network.AddReplaceItems(
                [
                    ..this.Inputs.WhereNotNull()
                ]
            );
        }

        public IEnumerable<T?> Inputs { get; }

        public ReadOnlyNetwork Network => this.network;
    }
}
