using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Divider
    (
        Divider.ParameterInfo parameters,
        InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        UngroupedOperationBase
        <
            Divider.ParameterInfo,
            InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        IUngroupedOperationStatic
        <
            Divider,
            Divider.ParameterInfo,
            InterneuronSet
        >,
        IOperationStatic
        <
            Divider,
            Divider.ParameterInfo,
            InterneuronSet
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

        public static Divider.ParameterInfo GetDefaultParameters() =>
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
            Divider.ParameterInfo parameters,
            VariableInfo variableInfo
        )
        {
            var result = new List<ReadOnlyNetwork>();

            if
            (
                AndGate.TryCreate
                (
                    out var AND___Dividend__Divisor,
                    new(
                        new(
                            parameters.Inputs.Dividend,
                            parameters.Inputs.Divisor
                        ),
                        new(
                            parameters.Outputs.Quotient
                        )
                    ),
                    InterneuronTagInfo.CreateByCommonTagPrefix
                    (
                        variableInfo.Inputs.First(),
                        2
                    )
                )
            )
                result.Add(AND___Dividend__Divisor.Network);

            return new(result);
        }
    }
}
