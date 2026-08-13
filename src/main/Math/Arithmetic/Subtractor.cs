using ei8.Cortex.Coding.d23.Math.Logic;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class Subtractor : OperationBase, IOperation<Subtractor>
    {
        public enum Input
        {
            Minuend,
            Subtrahend,
            PrecedingBorrow
        }

        public enum Output
        {
            Difference,
            Borrow
        }

        protected Subtractor(
            FunctionalParameter<BinaryNeuronParameter> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) : base(
            parameters,
            networks,
            variableInfo
        )
        {
        }

        public static Subtractor Create(
            FunctionalParameter<BinaryNeuronParameter> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) => new(
            parameters, 
            networks, 
            variableInfo
        );

        public static FunctionalParameter<BinaryNeuronParameter> GetDefaultParameters(
            BinaryNeuronParameter? precedingValue,
            int exponent
        ) => new(
            [
                BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Input.Minuend)}"),
                BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Input.Subtrahend)}"),
                precedingValue
            ],
            [
                BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Output.Difference)}"),
                BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Output.Borrow)}")
            ]
        );

        public static IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworks(
            FunctionalParameter<BinaryNeuronParameter> parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        )
        {
            var result = new List<IneurUL>();
            string subtractorName = variableInfo.Inputs.First();
            var precedingBorrow = parameters.Inputs.ElementAt((int)Input.PrecedingBorrow);

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
                            parameters.Inputs.ElementAt((int) Input.Minuend),
                            parameters.Inputs.ElementAt((int) Input.Subtrahend),
                            half1_XOR_Result,
                            half1_OUT___Half1_NOT___Minuend,
                            half1_Borrow,
                            subtractorName
                        )
                    );

                    // half2 interneurons
                    if (
                        DualInputLogicGateBase.TryCreate(
                            out XorGate? half2_XOR___Borrow__Half1_XOR_Result,
                            new(
                                precedingBorrow,
                                half1_XOR_Result,
                                parameters.Outputs.ElementAt((int) Output.Difference)
                            ),
                            new(
                                [
                                    precedingSubtractorName,
                                    subtractorName,
                                ],
                                subtractorName
                            )
                        ) &&
                        NotGate.TryCreate(
                            out NotGate? half2_NOT___Half1_XOR_Result,
                            new(
                                half1_XOR_Result,
                                half2_OUT___Half2_NOT___Half1_XOR_Result
                            ),
                            new SingleInputInterneuronTagInfo(subtractorName)
                        ) &&
                        DualInputLogicGateBase.TryCreate(
                            out AndGate? half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result,
                            new(
                                precedingBorrow,
                                half2_OUT___Half2_NOT___Half1_XOR_Result,
                                half2_Borrow
                            ),
                            new(
                                [
                                    precedingSubtractorName,
                                    subtractorName
                                ],
                                subtractorName
                            )
                        ) &&
                        // OR Borrows
                        DualInputLogicGateBase.TryCreate(
                            out OrGate? OR___Half1_Borrow__Half2_Borrow,
                            new(
                                half1_Borrow,
                                half2_Borrow,
                                parameters.Outputs.ElementAt((int) Output.Borrow)
                            ),
                            new DualInputInterneuronTagInfo(subtractorName)
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
                            parameters.Inputs.ElementAt((int)Input.Minuend),
                            parameters.Inputs.ElementAt((int)Input.Subtrahend),
                            parameters.Outputs.ElementAt((int)Output.Difference),
                            half1_OUT___Half1_NOT___Minuend,
                            parameters.Outputs.ElementAt((int)Output.Borrow),
                            subtractorName
                        )
                    );
                }
            }

            return result.Select(n => n.Network);
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
                DualInputLogicGateBase.TryCreate(
                    out XorGate? half1_XOR___Minuend__Subtrahend,
                    new(
                        minuend,
                        subtrahend,
                        xorOutput
                    ),
                    new DualInputInterneuronTagInfo(prefix)
                ) &&
                NotGate.TryCreate(
                    out NotGate? half1_NOT___Minuend,
                    new(
                        minuend,
                        notOutput
                    ),
                    new SingleInputInterneuronTagInfo(prefix)
                ) &&
                DualInputLogicGateBase.TryCreate(
                    out AndGate? half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend,
                    new(
                        notOutput,
                        subtrahend,
                        andOutput
                    ),
                    new DualInputInterneuronTagInfo(prefix)
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
