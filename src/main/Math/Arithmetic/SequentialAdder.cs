using ei8.Cortex.Coding.d23.Sequences;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class SequentialAdder
    (
        SequentialAdder.AugmentationInfo augmentation,
        Next next,
        Adder adder,
        VariableInfo? variableInfo
    ) : 
        CompositeCircuitBase
        <
            Next, 
            Adder
        >
        (
            next,
            adder,
            variableInfo
        ),
        IAugmentedCompositeCircuit
        <
            SequentialAdder,
            SequentialAdder.AugmentationInfo,
            Next,
            Adder
        >
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            [.. base.GetNetworks(), this.Augmentation.Network ];

        public static SequentialAdder Create
        (
            AugmentationInfo augmentation,
            Next next,
            Adder adder,
            VariableInfo? variableInfo
        ) =>
            new
            (
                augmentation,
                next,
                adder,
                variableInfo
            );

        public static bool TryCreate<T>
        (
            [NotNullWhen(true)] out T? result,
            Next next,
            Adder adder,
            float inputStrength,
            VariableInfo? precedingVariableInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
            where T :
                IAugmentedCompositeCircuit
                <
                    T,
                    SequentialAdder.AugmentationInfo,
                    Next,
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
                ArgumentNullException.ThrowIfNull(adder.Parameters.Outputs.CarryOver);

                result = T.Create
                (
                    new
                    (
                        NetworkHelper.LinkInputNeuronsToInterneuron
                        (
                            next.Interneurons.Interneuron.GetInterneuron(),
                            (
                                (IEnumerable<IneurUL>)
                                [
                                    adder.Parameters.Outputs.Sum,
                                    adder.Parameters.Outputs.CarryOver
                                ]
                            ).Combine().GetItems<Neuron>().Select(n => new NeuronInfo(n, NeurotransmitterEffect.Excite, inputStrength)),
                            NetworkHelper.InputNeuronStrengthMode.Manual
                        )
                    ),
                    next,
                    adder,
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }

        public SequentialAdder.AugmentationInfo Augmentation { get; } = augmentation;

        public Next Next => this.Circuit1;

        public Adder Adder => this.Circuit2;
    }
}
