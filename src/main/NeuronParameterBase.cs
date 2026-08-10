using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23
{
    public abstract class NeuronParameterBase : IVariable
    {
        protected Network network;

        protected NeuronParameterBase(IEnumerable<NeuronInfo> neuronInfos)
        {
            this.NeuronInfos = neuronInfos;

            this.network = new();
            this.network.AddReplaceItems(
                this.NeuronInfos.Select(ni => ni.Neuron)
            );
        }

        public IEnumerable<NeuronInfo> NeuronInfos { get; }

        public ReadOnlyNetwork Network => this.network;

        public VariableInfo? VariableInfo { get; protected set; }
    }
}
