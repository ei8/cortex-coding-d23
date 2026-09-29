using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Sequences
{
    public partial class BiphasicNext
    (
        Next.ParameterInfo parameters,
        BiphasicNext.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        BiphasicAdjacentBase
        <
            Next.ParameterInfo,
            BiphasicNext.InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        IBiphasicAdjacentStatic
        <
            BiphasicNext,
            Next.ParameterInfo,
            BiphasicNext.InterneuronSet
        >,
        IAdjacentStatic
        <
            BiphasicNext,
            Next.ParameterInfo,
            BiphasicNext.InterneuronSet
        >
    {
        public static BiphasicNext Create
        (
            Next.ParameterInfo parameters,
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
            Next.ParameterInfo parameters,
            VariableInfo variableInfo,
            float inputStrength = 0.5f,
            InterneuronSet? precedingInterneurons = null,
            params NeuronInfo[] additionalInputs
        ) =>
            BiphasicNext.CreateInterneurons
            (
                parameters,
                variableInfo,
                inputStrength,
                precedingInterneurons,
                1f,
                additionalInputs
            );

        public static InterneuronSet CreateInterneurons
        (
            Next.ParameterInfo parameters,
            VariableInfo variableInfo,
            float inputStrength = 0.5f,
            InterneuronSet? precedingInterneurons = null,
            float interPhaseStrength = 1f,
            params NeuronInfo[] additionalInputs
        )
        {
            ArgumentNullException.ThrowIfNull(parameters.Outputs.Next);
            ArgumentNullException.ThrowIfNull(parameters.Inputs.Function);
            ArgumentNullException.ThrowIfNull(parameters.Inputs.Current);

            var completer = NetworkHelper.CreateInterneuronNetworkByOutputNeurons
            (
                $"{variableInfo.Function}.{nameof(InterneuronSet.Completer)}({variableInfo.Inputs.First()})",
                parameters.Outputs.Next.Neuron
            );

            var initiator = NetworkHelper.CreateInterneuronNetworkByOutputNeurons
            (
                $"{variableInfo.Function}.{nameof(InterneuronSet.Initiator)}({variableInfo.Inputs.First()})",
                interPhaseStrength,
                completer.GetInterneuron()
            );

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
                        precedingInterneurons.Initiator.GetInterneuron(),
                        NeurotransmitterEffect.Inhibit,
                        1f
                    )
                );

            var interneuronFromInputsToInitiator = NetworkHelper.LinkInputNeuronsToInterneuron(
                initiator.GetInterneuron(),
                [.. inputNeurons],
                NetworkHelper.InputNeuronStrengthMode.Manual,
                additionalInputs
            );
            
            initiator = ((IEnumerable<ReadOnlyNetwork>) [initiator, interneuronFromInputsToInitiator]).Combine();

            return new(initiator, completer);
        }
    }
}
