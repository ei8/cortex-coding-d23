using System.Linq;

namespace ei8.Cortex.Coding.d23.Collections
{
    public class Next : AdjacentBase<Next.Input, Next.Output>, IAdjacent<Next, Next.Input, Next.Output>
    {
        public class Input(UnaryNeuronParameter? current) : InputCircuitParameterSubset<UnaryNeuronParameter>(current)
        {
            public UnaryNeuronParameter? Current => this.Parameter1;
        }

        public class Output(UnaryNeuronParameter? next) : OutputCircuitParameterSubset<UnaryNeuronParameter>(next)
        {
            public UnaryNeuronParameter? Next => this.Parameter1;
        }

        protected Next(
            FunctionalCircuitParameter<Input, Output> parameters,
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
            FunctionalCircuitParameter<Input, Output> parameters,
            ReadOnlyNetwork interneuronNetwork,
            ReadOnlyNetwork linkedInputNeurons,
            VariableInfo? variableInfo
        ) => new(
            parameters,
            interneuronNetwork,
            linkedInputNeurons,
            variableInfo
        );

        public static ReadOnlyNetwork CreateInterneuronNetwork(
            FunctionalCircuitParameter<Input, Output> parameters,
            VariableInfo variableInfo
        ) => NetworkHelper.CreateInterneuronNetworkByOutputNeurons(
            $"{variableInfo.Function}({variableInfo.Inputs.First()})",
            [parameters.Outputs.Next!.Neuron]
        );

        public static ReadOnlyNetwork LinkInputNeurons(
            ReadOnlyNetwork interneuronNetwork,
            FunctionalCircuitParameter<Input, Output> parameters,
            ReadOnlyNetwork? precedingInterneuronNetwork = null,
            params Neuron[] additionalInputs
        ) => AdjacentBase<Next.Input, Next.Output>.LinkInputNeurons(
            parameters.Inputs.Current!,
            interneuronNetwork,
            precedingInterneuronNetwork,
            additionalInputs
        );
    }
}
