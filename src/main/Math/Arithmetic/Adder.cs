using ei8.Cortex.Coding.d23.Math.Logic;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class Adder : FunctionalCircuitBase<BinaryNeuronInfo>
    {
        public enum Input
        {
            Addend1,
            Addend2,
            PrecedingCarryOver
        }

        public enum Output
        {
            Sum,
            CarryOver
        }

        public Adder(
            int exponent = 0,
            BinaryNeuronInfo? precedingCarryOver = null
        ) :
        this(
            new(
                [
                    BinaryNeuronInfo.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Input.Addend1)}"),
                    BinaryNeuronInfo.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Input.Addend2)}"),
                    precedingCarryOver
                ],
                [
                    BinaryNeuronInfo.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Output.Sum)}"),
                    BinaryNeuronInfo.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Output.CarryOver)}")
                ]
            ),
            exponent
        )
        {
        }

        public Adder(
            FunctionalParameter<BinaryNeuronInfo> parameters,
            int exponent = 0
        )
        {
            this.network.AddReplaceItems(
                this.Parameters = parameters
            );

            string adderName = $"{nameof(Adder)}{exponent + 1}";
            var precedingCarryOver = parameters.Inputs.ElementAt((int)Input.PrecedingCarryOver);

            if (
                // is not least significant bit
                precedingCarryOver != null &&
                BinaryNeuronInfo.TryCreate(out var half1_XOR_Result, adderName) &&
                BinaryNeuronInfo.TryCreate(out var half1_CarryOver, adderName) &&
                BinaryNeuronInfo.TryCreate(out var half2_CarryOver, adderName)
            )
            {
                this.network.AddReplaceItems(
                    half1_XOR_Result,
                    half1_CarryOver,
                    half2_CarryOver
                );

                string precedingAdderName = $"{nameof(Adder)}{exponent}";

                Adder.CreateAdderHalf1Interneurons(
                    this.network,
                    this.Parameters.Inputs.Take(2),
                    half1_XOR_Result,
                    half1_CarryOver,
                    adderName
                );

                // half2
                if (
                    LogicGateBase.TryCreate(
                        out XorGate? half2_XOR___CarryOver__Half1_XOR_Result,
                        new(
                            [
                                precedingCarryOver,
                                half1_XOR_Result
                            ],
                            [
                                this.Parameters.Outputs.ElementAt((int) Output.Sum)
                            ]
                        ),
                        new(
                            [
                                precedingAdderName,
                                adderName
                            ],
                            adderName
                        )
                    ) &&
                    LogicGateBase.TryCreate(
                        out AndGate? half2_AND___CarryOver__Half1_XOR_Result,
                        new(
                            [
                                precedingCarryOver,
                                half1_XOR_Result
                            ],
                            [
                                half2_CarryOver
                            ]
                        ),
                        new(
                            [
                                precedingAdderName,
                                adderName
                            ],
                            adderName
                        )
                    ) &&
                    // OR carryOvers
                    LogicGateBase.TryCreate(
                        out OrGate? OR___Half1_CarryOver__Half2_CarryOver,
                        new(
                            [
                                half1_CarryOver,
                                half2_CarryOver
                            ],
                            [
                                this.Parameters.Outputs.ElementAt((int) Output.CarryOver)
                            ]
                        ),
                        InterneuronTagInfo.CreateSameTagForDualInput(adderName)
                    )
                )
                {
                    this.network.AddReplaceItems(
                        half2_XOR___CarryOver__Half1_XOR_Result,
                        half2_AND___CarryOver__Half1_XOR_Result,
                        OR___Half1_CarryOver__Half2_CarryOver
                    );
                }
            }
            else
            {
                Adder.CreateAdderHalf1Interneurons(
                    this.network,
                    this.Parameters.Inputs.Take(2),
                    this.Parameters.Outputs.ElementAt((int) Output.Sum),
                    this.Parameters.Outputs.ElementAt((int) Output.CarryOver),
                    adderName
                );
            }
        }

        private static void CreateAdderHalf1Interneurons(
            Network network,
            IEnumerable<BinaryNeuronInfo?> addends,
            BinaryNeuronInfo? xorOutput,
            BinaryNeuronInfo? andOutput,
            string prefix
        )
        {
            // Link half1 interneurons
            if (
                LogicGateBase.TryCreate(
                    out XorGate? half1_XOR___Addend1__Addend2,
                    new(
                        addends,
                        [xorOutput]
                    ),
                    InterneuronTagInfo.CreateSameTagForDualInput(prefix)
                ) &&
                LogicGateBase.TryCreate(
                    out AndGate? half1_AND___Addend1__Addend2,
                    new(
                        addends,
                        [andOutput]
                    ),
                    InterneuronTagInfo.CreateSameTagForDualInput(prefix)
                )
            )
            {
                network.AddReplaceItems(
                    half1_XOR___Addend1__Addend2,
                    half1_AND___Addend1__Addend2
                );
            }
        }
    }
}
