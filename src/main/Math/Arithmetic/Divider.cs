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
        UngroupedOperationBase
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

            var result = new InterneuronSet();

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
                result.AND___Dividend__Divisor = AND___Dividend__Divisor;

            return result;
        }
    }
}
