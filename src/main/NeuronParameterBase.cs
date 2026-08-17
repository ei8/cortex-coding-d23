using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23
{
    public abstract class NeuronParameterBase
    (
        IEnumerable<NeuronInfo> neuronInfos, 
        VariableInfo? variableInfo
    ) : 
        neurULBase, 
        INeuronParameter
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            [this.NeuronInfos.Select(ni => ni.Neuron).ToNetwork()];

        public IEnumerable<NeuronInfo> NeuronInfos { get; } = neuronInfos;

        public VariableInfo? VariableInfo { get; } = variableInfo;
    }
}
