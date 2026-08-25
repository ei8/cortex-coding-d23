using ei8.Cortex.Coding.d23.Math.Logic;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Multiplier
    (
        FunctionalCircuitParameter<Multiplier.Input, Multiplier.Output> parameters,
        InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        UngroupedOperationBase
        <
            FunctionalCircuitParameter
            <
                Multiplier.Input,
                Multiplier.Output
            >,
            InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ), 
        IUngroupedOperation
        <
            Multiplier,
            FunctionalCircuitParameter<Multiplier.Input, Multiplier.Output>,
            InterneuronSet
        >
    {
        public static Multiplier Create
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

        public static FunctionalCircuitParameter<Input, Output> GetDefaultParameters(int exponent) => 
            new
            (
                new
                (
                    BinaryNeuronParameter.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Input.Multiplicand)}"),
                    BinaryNeuronParameter.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Input.Multiplier)}")
                ),
                new
                (
                    BinaryNeuronParameter.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Output.Product)}")
                )
            );

        public static InterneuronSet CreateInterneurons
        (
            FunctionalCircuitParameter<Input, Output> parameters,
            VariableInfo variableInfo
        )
        {
            var result = new List<ReadOnlyNetwork>();

            if
            (
                DualInputLogicGateBase.TryCreate
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
