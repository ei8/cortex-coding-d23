using ei8.Cortex.Coding.d23.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class SequentialAdder
    (
        SequentialAdder.AugmentationInfo augmentation,
        BiphasicNext biphasicNext,
        Adder adder,
        VariableInfo? variableInfo
    ) : 
        CompositeCircuitBase
        <
            BiphasicNext, 
            Adder
        >
        (
            biphasicNext,
            adder,
            variableInfo
        ),
        IAugmentedCompositeCircuit
        <
            SequentialAdder,
            SequentialAdder.AugmentationInfo,
            BiphasicNext,
            Adder
        >
    {
        public class AugmentationInfo(ReadOnlyNetwork initiatorPostsynapticTerminals, ReadOnlyNetwork completerPresynapticTerminals) : AugmentationBase
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
                [
                    this.InitiatorPostsynapticTerminals,
                    this.CompleterPresynapticTerminals
                ];

            public ReadOnlyNetwork InitiatorPostsynapticTerminals { get; } = initiatorPostsynapticTerminals;

            public ReadOnlyNetwork CompleterPresynapticTerminals { get; } = completerPresynapticTerminals;
        }

        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            [.. base.GetNetworks(), this.Augmentation.Network ];

        public static SequentialAdder Create
        (
            AugmentationInfo augmentation,
            BiphasicNext biphasicNext,
            Adder adder,
            VariableInfo? variableInfo
        ) =>
            new
            (
                augmentation,
                biphasicNext,
                adder,
                variableInfo
            );

        public static bool TryCreate<T>
        (
            [NotNullWhen(true)] out T? result,
            BiphasicNext biphasicNext,
            Adder adder,
            VariableInfo? precedingVariableInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
            where T :
                IAugmentedCompositeCircuit
                <
                    T,
                    SequentialAdder.AugmentationInfo,
                    BiphasicNext,
                    Adder
                >
        {
            bool bResult = false;
            result = default;
            // TODO: use variableInfo
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                ArgumentNullException.ThrowIfNull(adder.Parameters.Inputs.Addend1);
                ArgumentNullException.ThrowIfNull(adder.Parameters.Inputs.Addend2);
                ArgumentNullException.ThrowIfNull(adder.Parameters.Outputs.Sum);

                var initiatorInterneuron = biphasicNext.Interneurons.Initiator.GetInterneuron();
                var initiatorPostsynapticTerminals = NetworkHelper.CreateTerminals
                (
                    [initiatorInterneuron],
                    (
                        (IEnumerable<IneurUL>)
                        [
                            adder.Parameters.Inputs.Addend1,
                            adder.Parameters.Inputs.Addend2
                        ]
                    ).Combine().GetItems<Neuron>(),
                    NeurotransmitterEffect.Excite,
                    0.5f
                ).ToNetwork();

                var completerInterneuron = biphasicNext.Interneurons.Completer.GetInterneuron();
                var completerPresynapticTerminals = NetworkHelper.CreateTerminals
                (
                    (
                        (IEnumerable<IneurUL>)
                        [
                            adder.Parameters.Outputs.Sum
                        ]
                    ).Combine().GetItems<Neuron>(),
                    [completerInterneuron],
                    NeurotransmitterEffect.Excite,
                    0.5f
                ).ToNetwork();

                result = T.Create
                (
                    new
                    (
                        initiatorPostsynapticTerminals,
                        completerPresynapticTerminals
                    ),
                    biphasicNext,
                    adder,
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }

        public SequentialAdder.AugmentationInfo Augmentation { get; } = augmentation;
    }
}
