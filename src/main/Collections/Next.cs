using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Collections
{
    public class Next : AdjacentBase
    {
        public Next() : base()
        {
        }

        protected override IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworks(
            FunctionalParameter<UnaryNeuronParameter> parameters,
            VariableInfo variableInfo,
            params Neuron[] additionalInputs
        )
        {
            var interneuronNetworks = NetworkHelper.CreateInterneuronNetworkByOutputNeurons(
                $"{variableInfo.Function}({variableInfo.Inputs.First()})",
                [parameters.Outputs.First()!.Neuron]
            );
            return [
                interneuronNetworks,
                AdjacentBase.LinkInputNeuron(
                    parameters.Inputs.First()!,
                    interneuronNetworks,
                    additionalInputs
                )
            ];
        }
    }
}
