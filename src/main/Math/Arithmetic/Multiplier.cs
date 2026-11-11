using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Multiplier
    (
        Multiplier.ParameterInfo parameters,
        Multiplier.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TruthTableBase
        <
            Multiplier,
            Multiplier.ParameterInfo,
            Multiplier.Input,
            Multiplier.Output,
            Multiplier.InterneuronSet,
            TwoInputInterneuronTagger
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ITruthTableStatic
        <
            Multiplier,
            Multiplier.ParameterInfo,
            Multiplier.Input,
            Multiplier.Output,
            Multiplier.InterneuronSet,
            TwoInputInterneuronTagger
        >,
        IUngroupedOperationStatic
        <
            Multiplier,
            Multiplier.ParameterInfo,
            Multiplier.InterneuronSet
        >
    {
        public static Multiplier Create
        (
            Multiplier.ParameterInfo parameters,
            InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => 
            new 
            (
                parameters,
                interneurons,
                variableInfo
            );

        public static Multiplier.ParameterInfo CreateDefaultParameters() => 
            new
            (
                new
                (
                    BinaryNeuronParameter.Create($"{nameof(Multiplier)}.{nameof(Input.Multiplicand)}"),
                    BinaryNeuronParameter.Create($"{nameof(Multiplier)}.{nameof(Input.Multiplier)}")
                ),
                new
                (
                    BinaryNeuronParameter.Create($"{nameof(Multiplier)}.{nameof(Output.Product)}")
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

            InterneuronSet? result = null;

            if
            (
                AndGate.TryCreate
                (
                    out var AND___Multiplicand__Multiplier,
                    new
                    (
                        new
                        (
                            parameters.Inputs.Multiplicand,
                            parameters.Inputs.Multiplier
                        ),
                        new
                        (
                            parameters.Outputs.Product
                        )
                    ),
                    new
                    (
                        interneuronTagger.CreateByVariable(variableInfo)
                    )
                )
            )
                result = new InterneuronSet()
                {
                    AND___Multiplicand__Multiplier = AND___Multiplicand__Multiplier
                };

            ArgumentNullException.ThrowIfNull(result);

            return result;
        }
    }
}
