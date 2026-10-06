using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Multiplier
    (
        Multiplier.ParameterInfo parameters,
        InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        UngroupedOperationBase
        <
            Multiplier.ParameterInfo,
            InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ), 
        IUngroupedOperationStatic
        <
            Multiplier,
            Multiplier.ParameterInfo,
            InterneuronSet
        >,
        IOperationStatic
        <
            Multiplier,
            Multiplier.ParameterInfo,
            InterneuronSet
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

        public static Multiplier.ParameterInfo GetDefaultParameters(int exponent) => 
            new
            (
                new
                (
                    BinaryNeuronParameter.Create($"{nameof(Multiplier)}{exponent + 1}.{nameof(Input.Multiplicand)}"),
                    BinaryNeuronParameter.Create($"{nameof(Multiplier)}{exponent + 1}.{nameof(Input.Multiplier)}")
                ),
                new
                (
                    BinaryNeuronParameter.Create($"{nameof(Multiplier)}{exponent + 1}.{nameof(Output.Product)}")
                )
            );

        public static InterneuronSet CreateInterneurons
        (
            Multiplier.ParameterInfo parameters,
            VariableInfo variableInfo
        )
        {
            var result = new List<ReadOnlyNetwork>();

            if
            (
                TwoInputOneOutputLogicGateBase.TryCreate
                (
                    out AndGate? AND___Multiplicand__Multiplier,
                    new(
                        new(
                            parameters.Inputs.Multiplicand,
                            parameters.Inputs.Multiplier
                        ),
                        new(
                            parameters.Outputs.Product
                        )
                    ),
                    InterneuronTagInfo.CreateByCommonTagPrefix
                    (
                        variableInfo.Inputs.First(),
                        2
                    )
                )
            )
                result.Add(AND___Multiplicand__Multiplier.Network);

            return new(result);
        }
    }
}
