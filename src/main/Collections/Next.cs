using System.Linq;

namespace ei8.Cortex.Coding.d23.Collections
{
    public class Next : AdjacentBase, IAdjacent<Next>
    {
        protected Next(
            FunctionalParameter<UnaryNeuronParameter> parameters,
            ReadOnlyNetwork interneuronNetwork,
            ReadOnlyNetwork linkedInputNeurons,
            VariableInfo? variableInfo
        ) : base(
            parameters,
            interneuronNetwork,
            linkedInputNeurons,
            variableInfo
        )
        {
        }

        public static Next Create(
            FunctionalParameter<UnaryNeuronParameter> parameters,
            ReadOnlyNetwork interneuronNetwork,
            ReadOnlyNetwork linkedInputNeurons,
            VariableInfo? variableInfo
        ) => new(parameters, interneuronNetwork, linkedInputNeurons, variableInfo);

        public static ReadOnlyNetwork CreateInterneuronNetwork(
            FunctionalParameter<UnaryNeuronParameter> parameters,
            VariableInfo variableInfo
        ) => NetworkHelper.CreateInterneuronNetworkByOutputNeurons(
            $"{variableInfo.Function}({variableInfo.Inputs.First()})",
            [parameters.Outputs.First()!.Neuron]
        );

        public static ReadOnlyNetwork LinkInputNeurons(
            ReadOnlyNetwork interneuronNetwork,
            FunctionalParameter<UnaryNeuronParameter> parameters,
            params Neuron[] additionalInputs
        ) => AdjacentBase.LinkInputNeurons(
            // TODO: pass multiple inputs here, including inhibitor from previous Step
            parameters.Inputs.First()!,
            interneuronNetwork,
            additionalInputs
        );
    }
}
