using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Divider
    (
        Divider.ParameterInfo parameters,
        Divider.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TruthTableBase
        <
            Divider,
            Divider.ParameterInfo,
            Divider.Input,
            Divider.Output,
            Divider.InterneuronSet,
            TwoInputInterneuronTagger
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ITruthTableStatic
        <
            Divider,
            Divider.ParameterInfo,
            Divider.Input,
            Divider.Output,
            Divider.InterneuronSet,
            TwoInputInterneuronTagger
        >,
        IUngroupedOperationStatic
        <
            Divider,
            Divider.ParameterInfo,
            Divider.InterneuronSet
        >
    {
        public static Divider Create
        (
            Divider.ParameterInfo parameters,
            InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) =>
            new
            (
                parameters,
                interneurons,
                variableInfo
            );

        public static Divider.ParameterInfo CreateDefaultParameters() =>
            new
            (
                new
                (
                    BinaryNeuronParameter.Create($"{nameof(Divider)}.{nameof(Input.Dividend)}"),
                    BinaryNeuronParameter.Create($"{nameof(Divider)}.{nameof(Input.Divisor)}")
                ),
                new
                (
                    BinaryNeuronParameter.Create($"{nameof(Divider)}.{nameof(Output.Quotient)}"),
                    BinaryNeuronParameter.Create($"{nameof(Divider)}.{nameof(Output.Undefined)}")
                )
            );

        public static InterneuronSet CreateInterneurons
        (
            ParameterInfo parameters, 
            VariableInfo variableInfo,
            TwoInputInterneuronTagger? interneuronTagger = null,
            AdditionalInputNeuronInfo? additionalInputNeurons = null
        )
        {
            ArgumentNullException.ThrowIfNull(interneuronTagger);
            ArgumentNullException.ThrowIfNull(parameters.Outputs.Undefined);

            InterneuronSet? result = null;

            if
            (
                AndGate.TryCreate
                (
                    out var AND___Dividend__Divisor,
                    new
                    (
                        new
                        (
                            parameters.Inputs.Dividend,
                            parameters.Inputs.Divisor
                        ),
                        new
                        (
                            parameters.Outputs.Quotient
                        )
                    ),
                    new
                    (
                        interneuronTagger.CreateByVariable(variableInfo)
                    )
                )
            )
            {
                var interneuronsToUndefined = new Network();
                interneuronsToUndefined.AddReplaceItems(
                    [
                        NetworkHelper.CreateTerminal
                        (
                            AND___Dividend__Divisor.Interneurons.Interneuron1.GetInterneuron(),
                            parameters.Outputs.Undefined.Neuron1
                        ),
                        NetworkHelper.CreateTerminal
                        (
                            AND___Dividend__Divisor.Interneurons.Interneuron2.GetInterneuron(),
                            parameters.Outputs.Undefined.Neuron0
                        ),
                        NetworkHelper.CreateTerminal
                        (
                            AND___Dividend__Divisor.Interneurons.Interneuron3.GetInterneuron(),
                            parameters.Outputs.Undefined.Neuron1
                        ),
                        NetworkHelper.CreateTerminal
                        (
                            AND___Dividend__Divisor.Interneurons.Interneuron4.GetInterneuron(),
                            parameters.Outputs.Undefined.Neuron0
                        )
                    ]
                );

                result = new InterneuronSet()
                {
                    AND___Dividend__Divisor = AND___Dividend__Divisor,
                    InterneuronsToUndefined = interneuronsToUndefined
                };
            }

            ArgumentNullException.ThrowIfNull(result);

            return result;
        }
    }
}
