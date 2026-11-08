using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Adder
    (
        Adder.ParameterInfo parameters,
        Adder.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TruthTableBase
        <
            Adder,
            Adder.ParameterInfo,
            Adder.Input,
            Adder.Output,
            Adder.InterneuronSet,
            Adder.KeyedInterneuronTagger
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ITruthTableStatic
        <
            Adder,
            Adder.ParameterInfo,
            Adder.Input,
            Adder.Output,
            Adder.InterneuronSet,
            Adder.KeyedInterneuronTagger
        >,
        IGroupedOperationStatic
        <
            Adder,
            Adder.ParameterInfo,
            Adder.InterneuronSet
        >
    {
        public static Adder Create(
            Adder.ParameterInfo parameters,
            InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => new (
            parameters,
            interneurons,
            variableInfo
        );

        public static Adder.ParameterInfo CreateDefaultParameters(
            BinaryNeuronParameter? precedingValue,
            int exponent
        ) => new(
            new(
                BinaryNeuronParameter.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Input.Addend1)}"),
                BinaryNeuronParameter.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Input.Addend2)}"),
                precedingValue
            ),
            new(
                BinaryNeuronParameter.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Output.Sum)}"),
                BinaryNeuronParameter.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Output.CarryOver)}")
            )
        );

        public static InterneuronSet CreateInterneurons
        (
            ParameterInfo parameters, 
            VariableInfo variableInfo, 
            KeyedInterneuronTagger? interneuronTagger = null, 
            AdditionalInputNeuronInfo? additionalInputNeurons = null
        )
        {
            ArgumentNullException.ThrowIfNull(interneuronTagger);

            var result = new InterneuronSet();
            string adderName = variableInfo.Inputs.First();

            if (
                // is not least significant bit
                parameters.Inputs.PrecedingCarryOver != null &&
                BinaryNeuronParameter.TryCreate(out var half1_XOR_Result, adderName) &&
                BinaryNeuronParameter.TryCreate(out var half1_CarryOver, adderName) &&
                BinaryNeuronParameter.TryCreate(out var half2_CarryOver, adderName)
            )
            {
                Adder.CreateAdderHalf1Interneurons(
                    result,
                    variableInfo,
                    parameters.Inputs.Addend1,
                    parameters.Inputs.Addend2,
                    half1_XOR_Result,
                    half1_CarryOver,
                    additionalInputNeurons,
                    interneuronTagger
                );

                // half2
                if 
                (
                    XorGate.TryCreate
                    (
                        out var half2_XOR___CarryOver__Half1_XOR_Result,
                        new
                        (
                            new
                            (
                                parameters.Inputs.PrecedingCarryOver,
                                half1_XOR_Result
                            ),
                            new(parameters.Outputs.Sum)
                        ),
                        new
                        (
                            interneuronTagger.CreateByKey
                            (
                                nameof(InterneuronSet.Half2_XOR___CarryOver__Half1_XOR_Result),
                                variableInfo
                            )
                        )
                    ) &&
                    AndGate.TryCreate
                    (
                        out var half2_AND___CarryOver__Half1_XOR_Result,
                        new
                        (
                            new
                            (
                                parameters.Inputs.PrecedingCarryOver,
                                half1_XOR_Result
                            ),
                            new(half2_CarryOver)
                        ),
                        new
                        (
                            interneuronTagger.CreateByKey
                            (
                                nameof(InterneuronSet.Half2_AND___CarryOver__Half1_XOR_Result),
                                variableInfo
                            )
                        )
                    ) &&
                    // OR carryOvers
                    OrGate.TryCreate
                    (
                        out var OR___Half1_CarryOver__Half2_CarryOver,
                        new
                        (
                            new
                            (
                                half1_CarryOver,
                                half2_CarryOver
                            ),
                            new(parameters.Outputs.CarryOver)
                        ),
                        new
                        (
                            interneuronTagger.CreateByKey
                            (
                                nameof(InterneuronSet.OR___Half1_CarryOver__Half2_CarryOver),
                                variableInfo
                            )
                        )
                    )
                )
                {
                    result.Half2_XOR___CarryOver__Half1_XOR_Result = half2_XOR___CarryOver__Half1_XOR_Result;
                    result.Half2_AND___CarryOver__Half1_XOR_Result = half2_AND___CarryOver__Half1_XOR_Result;
                    result.OR___Half1_CarryOver__Half2_CarryOver = OR___Half1_CarryOver__Half2_CarryOver;
                }
            }
            else
            {
                Adder.CreateAdderHalf1Interneurons
                (
                    result,
                    variableInfo,
                    parameters.Inputs.Addend1,
                    parameters.Inputs.Addend2,
                    parameters.Outputs.Sum,
                    parameters.Outputs.CarryOver,
                    additionalInputNeurons,
                    interneuronTagger
                );
            }

            return result;
        }

        private static void CreateAdderHalf1Interneurons
        (
            InterneuronSet result,
            VariableInfo variableInfo,
            BinaryNeuronParameter? addend1,
            BinaryNeuronParameter? addend2,
            BinaryNeuronParameter? xorOutput,
            BinaryNeuronParameter? andOutput,
            AdditionalInputNeuronInfo? additionalInputNeurons,
            KeyedInterneuronTagger? interneuronTagger = null
        )
        {
            ArgumentNullException.ThrowIfNull(interneuronTagger);

            // Link half1 interneurons
            if 
            (
                XorGate.TryCreate
                (
                    out var half1_XOR___Addend1__Addend2,
                    new
                    (
                        new
                        (
                            addend1,
                            addend2
                        ),
                        new(xorOutput)
                    ),
                    new
                    (
                        interneuronTagger.CreateByKey
                        (
                            nameof(InterneuronSet.Half1_XOR___Addend1__Addend2),
                            variableInfo
                        )
                    ),
                    additionalInputNeurons
                ) &&
                AndGate.TryCreate
                (
                    out var half1_AND___Addend1__Addend2,
                    new
                    (
                        new
                        (
                            addend1,
                            addend2
                        ),
                        new(andOutput)
                    ),
                    new
                    (
                        interneuronTagger.CreateByKey
                        (
                            nameof(InterneuronSet.Half1_AND___Addend1__Addend2),
                            variableInfo
                        )
                    ),
                    additionalInputNeurons
                )
            )
            {
                result.Half1_XOR___Addend1__Addend2 = half1_XOR___Addend1__Addend2;
                result.Half1_AND___Addend1__Addend2 = half1_AND___Addend1__Addend2;
            }
        }
    }
}
