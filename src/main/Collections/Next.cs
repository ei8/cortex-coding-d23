using System;
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
            float inputStrength = 0.5f,
            InterneuronSet? precedingInterneurons = default,
            params NeuronInfo[] additionalInputs
        )
        {
            ArgumentNullException.ThrowIfNull(parameters.Outputs.Next);
            ArgumentNullException.ThrowIfNull(parameters.Inputs.Function);
            ArgumentNullException.ThrowIfNull(parameters.Inputs.Current);

            var interneuronToNext = NetworkHelper.CreateInterneuronNetworkByOutputNeurons(
                $"{variableInfo.Function}({variableInfo.Inputs.First()})",
                [parameters.Outputs.Next.Neuron]
            );

            interneuronToNext = ((IEnumerable<ReadOnlyNetwork>)
            [
                interneuronToNext,
                Next.LinkInputNeurons
                (
                    parameters,
                    interneuronToNext,
                    inputStrength,
                    precedingInterneurons,
                    additionalInputs
                )
            ]).Combine();

            return new InterneuronSet(interneuronToNext);
        }

        private static ReadOnlyNetwork LinkInputNeurons
        (
            FunctionalCircuitParameter<Input, Output> parameters,
            ReadOnlyNetwork interneuronToNext,
            float inputStrength,
            InterneuronSet? precedingInterneurons = default,
            params NeuronInfo[] additionalInputNeuronInfos
        )
        {
            ArgumentNullException.ThrowIfNull(parameters.Inputs.Function);
            ArgumentNullException.ThrowIfNull(parameters.Inputs.Current);

            var inputNeurons = new List<NeuronInfo>
            (
                [
                    new(parameters.Inputs.Function.Neuron, NeurotransmitterEffect.Excite, inputStrength),
                    new(parameters.Inputs.Current.Neuron, NeurotransmitterEffect.Excite, inputStrength)
                ]
            );
            if (precedingInterneurons != null)
                inputNeurons.Add
                (
                    new NeuronInfo
                    (
                        precedingInterneurons.Interneuron.GetInterneuron(),
                        NeurotransmitterEffect.Inhibit,
                        1f
                    )
                );

            return NetworkHelper.LinkInputNeuronsToInterneuron(
                interneuronToNext.GetInterneuron(),
                [.. inputNeurons],
                NetworkHelper.InputNeuronStrengthMode.Manual,
                additionalInputNeuronInfos
            );
        }
    }
}
