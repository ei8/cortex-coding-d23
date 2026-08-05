using System.Linq;

namespace ei8.Cortex.Coding.d23.Collections
{
    public class Next : AdjacentBase, IAdjacent<Next>
    {
        public enum Input
        {
            Current
        }

        public enum Output
        {
            Next,
        }

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
            [parameters.Outputs.ElementAt((int) Output.Next)!.Neuron]
        );

        public static ReadOnlyNetwork LinkInputNeurons(
            ReadOnlyNetwork interneuronNetwork,
            FunctionalParameter<UnaryNeuronParameter> parameters,
            ReadOnlyNetwork? precedingInterneuronNetwork = null,
            params Neuron[] additionalInputs
        ) => AdjacentBase.LinkInputNeurons(
            parameters.Inputs.ElementAt((int) Input.Current)!,
            interneuronNetwork,
            precedingInterneuronNetwork,
            additionalInputs
        );
    }
}
