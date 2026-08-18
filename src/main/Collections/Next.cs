using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Collections
{
    public class Next
    (
        FunctionalCircuitParameter
        <
            Next.Input, 
            Next.Output
        > 
        parameters,
        InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) : 
        AdjacentBase
        <
            FunctionalCircuitParameter
            <
                Next.Input, 
                Next.Output
            >,
            InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ), 
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
        public class Input
        (
            UnaryNeuronParameter? function, 
            UnaryNeuronParameter? current
        ) : 
            InputCircuitParameterSubset
            <
                UnaryNeuronParameter, 
                UnaryNeuronParameter
            >
            (
                function, 
                current
            )
        {
            public UnaryNeuronParameter? Function => this.Parameter1;
            public UnaryNeuronParameter? Current => this.Parameter2;
        }

        public class Output(UnaryNeuronParameter? subsequent) : OutputCircuitParameterSubset<UnaryNeuronParameter>(subsequent)
        {
            public UnaryNeuronParameter? Next => this.Parameter1;
        }

        public static Next Create
        (
            FunctionalCircuitParameter<Input, Output> parameters,
            InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => 
            new
            (
                parameters,
                interneurons,
                variableInfo
            );

        public static InterneuronSet CreateInterneurons
        (
            FunctionalCircuitParameter<Input, Output> parameters,
            VariableInfo variableInfo,
            InterneuronSet? precedingInterneurons = default,
            params Neuron[] additionalInputs
        )
        {
            var interneuronToNext = NetworkHelper.CreateInterneuronNetworkByOutputNeurons(
                $"{variableInfo.Function}({variableInfo.Inputs.First()})",
                [parameters.Outputs.Next!.Neuron]
            );

            interneuronToNext = ((IEnumerable<ReadOnlyNetwork>)
            [
                interneuronToNext,
                Next.LinkInputNeurons
                (
                    parameters.Inputs.Function!,
                    parameters.Inputs.Current!,
                    interneuronToNext,
                    precedingInterneurons,
                    additionalInputs
                )
            ]).Combine();

            return new InterneuronSet(interneuronToNext);
        }

        private static ReadOnlyNetwork LinkInputNeurons
        (
            UnaryNeuronParameter function,
            UnaryNeuronParameter current,
            ReadOnlyNetwork interneuronToNext,
            InterneuronSet? precedingInterneurons = default,
            params Neuron[] additionalInputs
        )
        {
            var inputNeurons = new List<NeuronInfo>
            (
                [
                    new(function.Neuron),
                    new(current.Neuron)
                ]
            );
            if (precedingInterneurons != null)
                inputNeurons.Add
                (
                    new NeuronInfo
                    (
                        precedingInterneurons.Interneuron.GetInterneuron(),
                        1f,
                        NeurotransmitterEffect.Inhibit
                    )
                );

            return NetworkHelper.LinkInputNeuronsToInterneuron(
                interneuronToNext.GetInterneuron(),
                [.. inputNeurons],
                additionalInputNeuronInfos: [.. additionalInputs.Select(n => new NeuronInfo(n))]
            );
        }
    }
}
