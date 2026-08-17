using System.Linq;

namespace ei8.Cortex.Coding.d23.Collections
{
    public class Next : 
        AdjacentBase
        <
            FunctionalCircuitParameter
            <
                Next.Input, 
                Next.Output
            >
        >, 
        IAdjacent
        <
            Next, 
            FunctionalCircuitParameter
            <
                Next.Input, 
                Next.Output
            >, 
            InterneuronSet
        >
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
            InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) : base(
            parameters,
            interneurons,
            variableInfo
        )
        {
        }

        public static Next Create(
            FunctionalCircuitParameter<Input, Output> parameters,
            InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => new(
            parameters,
            interneurons,
            variableInfo
        );

        public static InterneuronSet CreateInterneurons(
            FunctionalCircuitParameter<Input, Output> parameters,
            VariableInfo variableInfo,
            InterneuronSet? precedingInterneuronNetwork = default,
            params Neuron[] additionalInputs
        )
        {
            var interneuron1 = NetworkHelper.CreateInterneuronNetworkByOutputNeurons(
                $"{variableInfo.Function}({variableInfo.Inputs.First()})",
                [parameters.Outputs.Next!.Neuron]
            );

            return new InterneuronSet(
                interneuron1,
                AdjacentBase<FunctionalCircuitParameter<Next.Input, Next.Output>>.LinkInputNeurons(
                    parameters.Inputs.Current!,
                    interneuron1,
                    precedingInterneuronNetwork,
                    additionalInputs
                )
            );
        }
    }
}
