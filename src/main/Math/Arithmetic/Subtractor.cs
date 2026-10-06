using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Subtractor
    (
        Subtractor.ParameterInfo parameters,
        InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) : 
        GroupedOperationBase
        <
            Subtractor.ParameterInfo,
            InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ), 
        IGroupedOperationStatic
        <
            Subtractor, 
            Subtractor.ParameterInfo, 
            InterneuronSet
        >,
        IOperationStatic
        <
            Subtractor,
            Subtractor.ParameterInfo,
            InterneuronSet
        >
    {
        public static Subtractor Create(
            Subtractor.ParameterInfo parameters,
            InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => new(
            parameters, 
            interneurons, 
            variableInfo
        );

        public static Subtractor.ParameterInfo GetDefaultParameters(
            BinaryNeuronParameter? precedingValue,
            int exponent
        ) => new(
            new(
                BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Input.Minuend)}"),
                BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Input.Subtrahend)}"),
                precedingValue
            ),
            new(
                BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Output.Difference)}"),
                BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Output.Borrow)}")
            )
        );

        public static InterneuronSet CreateInterneurons(
            Subtractor.ParameterInfo parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        )
        {
            var result = new List<IneurUL>();
            string subtractorName = variableInfo.Inputs.First();
            var precedingBorrow = parameters.Inputs.PrecedingBorrow;

            // Declare Outputs
            if (BinaryNeuronParameter.TryCreate(out var half1_OUT___Half1_NOT___Minuend, subtractorName))
            {
                result.Add(half1_OUT___Half1_NOT___Minuend);
                if (
                    // is not least significant bit
                    precedingBorrow != null &&
                    precedingVariableInfo != null &&
                    BinaryNeuronParameter.TryCreate(out var half1_XOR_Result, subtractorName) &&
                    BinaryNeuronParameter.TryCreate(out var half2_OUT___Half2_NOT___Half1_XOR_Result, subtractorName) &&
                    BinaryNeuronParameter.TryCreate(out var half1_Borrow, subtractorName) &&
                    BinaryNeuronParameter.TryCreate(out var half2_Borrow, subtractorName)
                )
                {
                    result.AddRange(
                        [
                            half1_XOR_Result,
                            half2_OUT___Half2_NOT___Half1_XOR_Result,
                            half1_Borrow,
                            half2_Borrow
                        ]
                    );

                    var precedingSubtractorName = precedingVariableInfo.Inputs.First();

                    // half1 interneurons
                    result.AddRange(
                        Subtractor.CreateSubtractorHalf1Interneurons(
                            parameters.Inputs.Minuend,
                            parameters.Inputs.Subtrahend,
                            half1_XOR_Result,
                            half1_OUT___Half1_NOT___Minuend,
                            half1_Borrow,
                            subtractorName
                        )
                    );

                    // half2 interneurons
                    if (
                        TwoInputOneOutputLogicGateBase.TryCreate(
                            out XorGate? half2_XOR___Borrow__Half1_XOR_Result,
                            new(
                                new(
                                    precedingBorrow,
                                    half1_XOR_Result
                                ),
                                new(
                                    parameters.Outputs.Difference
                                )
                            ),
                            new(
                                subtractorName,
                                precedingSubtractorName,
                                subtractorName
                            )
                        ) &&
                        NotGate.TryCreate(
                            out NotGate? half2_NOT___Half1_XOR_Result,
                            new(
                                new(
                                    half1_XOR_Result
                                ),
                                new(
                                    half2_OUT___Half2_NOT___Half1_XOR_Result
                                )
                            ),
                            InterneuronTagInfo.CreateByCommonTagPrefix(subtractorName)
                        ) &&
                        TwoInputOneOutputLogicGateBase.TryCreate(
                            out AndGate? half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result,
                            new(
                                new(
                                    precedingBorrow,
                                    half2_OUT___Half2_NOT___Half1_XOR_Result
                                ),
                                new(
                                    half2_Borrow
                                )
                            ),
                            new(
                                subtractorName,
                                precedingSubtractorName,
                                subtractorName
                            )
                        ) &&
                        // OR Borrows
                        TwoInputOneOutputLogicGateBase.TryCreate(
                            out OrGate? OR___Half1_Borrow__Half2_Borrow,
                            new(
                                new(
                                    half1_Borrow,
                                    half2_Borrow
                                ),
                                new(
                                    parameters.Outputs.Borrow
                                )
                            ),
                            InterneuronTagInfo.CreateByCommonTagPrefix(subtractorName, 2)
                        )
                    )
                    {
                        result.AddRange(
                            [
                                half2_XOR___Borrow__Half1_XOR_Result,
                                half2_NOT___Half1_XOR_Result,
                                half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result,
                                OR___Half1_Borrow__Half2_Borrow
                            ]
                        );
                    }
                }
                else
                {
                    // half1
                    result.AddRange(
                        Subtractor.CreateSubtractorHalf1Interneurons(
                            parameters.Inputs.Minuend,
                            parameters.Inputs.Subtrahend,
                            parameters.Outputs.Difference,
                            half1_OUT___Half1_NOT___Minuend,
                            parameters.Outputs.Borrow,
                            subtractorName
                        )
                    );
                }
            }

            return new InterneuronSet(result.Select(n => n.Network));
        }

        private static IEnumerable<IneurUL> CreateSubtractorHalf1Interneurons(
            BinaryNeuronParameter? minuend,
            BinaryNeuronParameter? subtrahend,
            BinaryNeuronParameter? xorOutput,
            BinaryNeuronParameter? notOutput,
            BinaryNeuronParameter? andOutput,
            string prefix
        )
        {
            var result = new List<IneurUL>();
            // Link half1 interneurons
            if (
                TwoInputOneOutputLogicGateBase.TryCreate(
                    out XorGate? half1_XOR___Minuend__Subtrahend,
                    new(
                        new(
                            minuend,
                            subtrahend
                        ),
                        new(
                            xorOutput
                        )
                    ),
                    InterneuronTagInfo.CreateByCommonTagPrefix(prefix, 2)
                ) &&
                NotGate.TryCreate(
                    out NotGate? half1_NOT___Minuend,
                    new(
                        new(
                            minuend
                        ),
                        new(
                            notOutput
                        )
                    ),
                    InterneuronTagInfo.CreateByCommonTagPrefix(prefix)
                ) &&
                TwoInputOneOutputLogicGateBase.TryCreate(
                    out AndGate? half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend,
                    new(
                        new(
                            notOutput,
                            subtrahend
                        ),
                        new(
                            andOutput
                        )
                    ),
                    InterneuronTagInfo.CreateByCommonTagPrefix(prefix, 2)
                )
            )
            {
                result.AddRange(
                    [
                        half1_XOR___Minuend__Subtrahend,
                        half1_NOT___Minuend,
                        half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend
                    ]
                );
            }
            return result;
        }
    }
}
