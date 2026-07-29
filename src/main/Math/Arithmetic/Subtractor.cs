using ei8.Cortex.Coding.d23.Math.Logic;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class Subtractor : FunctionalCircuitBase<BinaryNeuronInfo>
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

        public Subtractor(
            int exponent = 0,
            BinaryNeuronInfo? precedingBorrow = null
        ) :
        this(
            new(
                [
                    BinaryNeuronInfo.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Input.Minuend)}"), 
                    BinaryNeuronInfo.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Input.Subtrahend)}"),
                    precedingBorrow
                ],
                [
                    BinaryNeuronInfo.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Output.Difference)}"),
                    BinaryNeuronInfo.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Output.Borrow)}")
                ]
            ),
            exponent
        )
        {
        }

        public Subtractor(
            FunctionalParameter<BinaryNeuronInfo> parameters,
            int exponent = 0
        )
        {
            this.network.AddReplaceItems(
                this.Parameters = parameters
            );

            string subtractorName = $"{nameof(Subtractor)}{exponent + 1}";
            var precedingBorrow = parameters.Inputs.ElementAt((int)Input.PrecedingBorrow);

            // Declare Outputs
            if (BinaryNeuronInfo.TryCreate(out var half1_OUT___Half1_NOT___Minuend, subtractorName))
            {
                this.network.AddReplaceItems(half1_OUT___Half1_NOT___Minuend);
                if (
                    // is not least significant bit
                    precedingBorrow != null &&
                    BinaryNeuronInfo.TryCreate(out var half1_XOR_Result, subtractorName) &&
                    BinaryNeuronInfo.TryCreate(out var half2_OUT___Half2_NOT___Half1_XOR_Result, subtractorName) &&
                    BinaryNeuronInfo.TryCreate(out var half1_Borrow, subtractorName) &&
                    BinaryNeuronInfo.TryCreate(out var half2_Borrow, subtractorName)
                )
                {
                    this.network.AddReplaceItems(
                        half1_XOR_Result,
                        half2_OUT___Half2_NOT___Half1_XOR_Result,
                        half1_Borrow,
                        half2_Borrow
                    );

                    string precedingSubtractorName = $"{nameof(Subtractor)}{exponent}";

                    // half1 interneurons
                    Subtractor.CreateSubtractorHalf1Interneurons(
                        this.network,
                        this.Parameters.Inputs.Take(2),
                        half1_XOR_Result,
                        half1_OUT___Half1_NOT___Minuend,
                        half1_Borrow,
                        subtractorName
                    );

                    // half2 interneurons
                    if (
                        LogicGateBase.TryCreate(
                            out XorGate? half2_XOR___Borrow__Half1_XOR_Result,
                            new(
                                [
                                    precedingBorrow,
                                    half1_XOR_Result
                                ],
                                [
                                    this.Parameters.Outputs.ElementAt((int) Output.Difference)
                                ]
                            ),
                            new(
                                [
                                    precedingSubtractorName,
                                    subtractorName,
                                ],
                                subtractorName
                            )
                        ) &&
                        LogicGateBase.TryCreate(
                            out NotGate? half2_NOT___Half1_XOR_Result,
                            new(
                                [half1_XOR_Result],
                                [half2_OUT___Half2_NOT___Half1_XOR_Result]
                            ),
                            InterneuronTagInfo.CreateSameTagForSingleInput(subtractorName)
                        ) &&
                        LogicGateBase.TryCreate(
                            out AndGate? half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result,
                            new(
                                [
                                    precedingBorrow,
                                    half2_OUT___Half2_NOT___Half1_XOR_Result
                                ],
                                [ 
                                    half2_Borrow 
                                ]
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
                        LogicGateBase.TryCreate(
                            out OrGate? OR___Half1_Borrow__Half2_Borrow,
                            new(
                                [
                                    half1_Borrow,
                                    half2_Borrow
                                ],
                                [
                                    this.Parameters.Outputs.ElementAt((int) Output.Borrow)
                                ]
                            ),
                            InterneuronTagInfo.CreateSameTagForDualInput(subtractorName)
                        )
                    )
                    {
                        this.network.AddReplaceItems(
                            half2_XOR___Borrow__Half1_XOR_Result,
                            half2_NOT___Half1_XOR_Result,
                            half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result,
                            OR___Half1_Borrow__Half2_Borrow
                        );
                    }
                }
                else
                {
                    // half1
                    Subtractor.CreateSubtractorHalf1Interneurons(
                        this.network,
                        this.Parameters.Inputs.Take(2),
                        this.Parameters.Outputs.ElementAt((int)Output.Difference),
                        half1_OUT___Half1_NOT___Minuend,
                        this.Parameters.Outputs.ElementAt((int)Output.Borrow),
                        subtractorName
                    );
                }
            }
        }

        private static void CreateSubtractorHalf1Interneurons(
            Network network,
            IEnumerable<BinaryNeuronInfo?> inputs,
            BinaryNeuronInfo? xorOutput,
            BinaryNeuronInfo? notOutput,
            BinaryNeuronInfo? andOutput,
            string prefix
        )
        {
            // Link half1 interneurons
            if (
                LogicGateBase.TryCreate(
                    out XorGate? half1_XOR___Minuend__Subtrahend,
                    new(
                        inputs,
                        [
                            xorOutput
                        ]
                    ),
                    InterneuronTagInfo.CreateSameTagForDualInput(prefix)
                ) &&
                LogicGateBase.TryCreate(
                    out NotGate? half1_NOT___Minuend,
                    new(
                        [
                            inputs.ElementAt((int) Input.Minuend)
                        ],
                        [
                            notOutput
                        ]
                    ),
                    InterneuronTagInfo.CreateSameTagForSingleInput(prefix)
                ) &&
                LogicGateBase.TryCreate(
                    out AndGate? half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend,
                    new(
                        [
                            notOutput,
                            inputs.ElementAt((int) Input.Subtrahend)
                        ],
                        [
                            andOutput
                        ]
                    ),
                    InterneuronTagInfo.CreateSameTagForDualInput(prefix)
                )
            )
            {
                network.AddReplaceItems(
                    half1_XOR___Minuend__Subtrahend,
                    half1_NOT___Minuend,
                    half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend
                );
            }
        }
    }
}
