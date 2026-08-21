using ei8.Cortex.Coding.d23.Math.Logic;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Adder
    (
        FunctionalCircuitParameter<Adder.Input, Adder.Output> parameters,
        InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        OperationBase
        <
            FunctionalCircuitParameter
            <
                Adder.Input,
                Adder.Output
            >,
            InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ), 
        IOperation
        <
            Adder,
            FunctionalCircuitParameter<Adder.Input, Adder.Output>,
            InterneuronSet
        >
    {
        public static Adder Create(
            FunctionalCircuitParameter<Input, Output> parameters,
            InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => new (
            parameters,
            interneurons,
            variableInfo
        );

        public static FunctionalCircuitParameter<Input, Output> GetDefaultParameters(
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

        public static InterneuronSet CreateInterneurons(
            FunctionalCircuitParameter<Input, Output> parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        ) => Adder.CreateInterneuronNetworksCore(
            variableInfo,
            precedingVariableInfo,
            parameters.Inputs.PrecedingCarryOver,
            parameters.Inputs.Addend1,
            parameters.Inputs.Addend2,
            parameters.Outputs.Sum,
            parameters.Outputs.CarryOver
        );

        internal static InterneuronSet CreateInterneuronNetworksCore(
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo,
            BinaryNeuronParameter? precedingCarryOver,
            BinaryNeuronParameter? addend1,
            BinaryNeuronParameter? addend2,
            BinaryNeuronParameter? sum,
            BinaryNeuronParameter? carryOver,
            NetworkHelper.InputNeuronStrengthMode additionalInputNeuronType = NetworkHelper.InputNeuronStrengthMode.And,
            params Neuron[] additionalInputs
        )
        {
            var result = new List<IneurUL>();
            string adderName = variableInfo.Inputs.First();

            if (
                // is not least significant bit
                precedingCarryOver != null &&
                precedingVariableInfo != null &&
                BinaryNeuronParameter.TryCreate(out var half1_XOR_Result, adderName) &&
                BinaryNeuronParameter.TryCreate(out var half1_CarryOver, adderName) &&
                BinaryNeuronParameter.TryCreate(out var half2_CarryOver, adderName)
            )
            {
                result.AddRange(
                    [
                        half1_XOR_Result,
                        half1_CarryOver,
                        half2_CarryOver
                    ]
                );

                string precedingAdderName = precedingVariableInfo.Inputs.First();

                result.AddRange(
                    Adder.CreateAdderHalf1Interneurons(
                        addend1,
                        addend2,
                        half1_XOR_Result,
                        half1_CarryOver,
                        adderName,
                        additionalInputNeuronType,
                        additionalInputs
                    )
                );

                // half2
                if (
                    DualInputLogicGateBase.TryCreate(
                        out XorGate? half2_XOR___CarryOver__Half1_XOR_Result,
                        new(
                            new(
                                precedingCarryOver,
                                half1_XOR_Result
                            ),
                            new(
                                sum
                            )
                        ),
                        new(
                            adderName,
                            precedingAdderName,
                            adderName
                        )
                    ) &&
                    DualInputLogicGateBase.TryCreate(
                        out AndGate? half2_AND___CarryOver__Half1_XOR_Result,
                        new(
                            new(
                                precedingCarryOver,
                                half1_XOR_Result
                            ),
                            new(
                                half2_CarryOver
                            )
                        ),
                        new(
                            adderName,
                            precedingAdderName,
                            adderName
                        )
                    ) &&
                    // OR carryOvers
                    DualInputLogicGateBase.TryCreate(
                        out OrGate? OR___Half1_CarryOver__Half2_CarryOver,
                        new(
                            new(
                                half1_CarryOver,
                                half2_CarryOver
                            ),
                            new(
                                carryOver
                            )
                        ),
                        InterneuronTagInfo.CreateByCommonTagPrefix(adderName, 2)
                    )
                )
                {
                    result.AddRange(
                        [
                            half2_XOR___CarryOver__Half1_XOR_Result,
                            half2_AND___CarryOver__Half1_XOR_Result,
                            OR___Half1_CarryOver__Half2_CarryOver
                        ]
                    );
                }
            }
            else
            {
                result.AddRange(
                    Adder.CreateAdderHalf1Interneurons(
                        addend1,
                        addend2,
                        sum,
                        carryOver,
                        adderName,
                        additionalInputNeuronType,
                        additionalInputs
                    )
                );
            }

            return new InterneuronSet(result.Select(n => n.Network));
        }

        private static IEnumerable<IneurUL> CreateAdderHalf1Interneurons(
            BinaryNeuronParameter? addend1,
            BinaryNeuronParameter? addend2,
            BinaryNeuronParameter? xorOutput,
            BinaryNeuronParameter? andOutput,
            string prefix,
            NetworkHelper.InputNeuronStrengthMode additionalInputNeuronType = NetworkHelper.InputNeuronStrengthMode.And,
            params Neuron[] additionalInputs
        )
        {
            var result = new List<IneurUL>();
            // Link half1 interneurons
            if (
                DualInputLogicGateBase.TryCreate(
                    out XorGate? half1_XOR___Addend1__Addend2,
                    new(
                        new(
                            addend1,
                            addend2
                        ),
                        new(
                            xorOutput
                        )
                    ),
                    InterneuronTagInfo.CreateByCommonTagPrefix(prefix, 2),
                    additionalInputNeuronType: additionalInputNeuronType,
                    additionalInputs: additionalInputs
                ) &&
                DualInputLogicGateBase.TryCreate(
                    out AndGate? half1_AND___Addend1__Addend2,
                    new(
                        new(
                            addend1,
                            addend2
                        ),
                        new(
                            andOutput
                        )
                    ),
                    InterneuronTagInfo.CreateByCommonTagPrefix(prefix, 2),
                    additionalInputNeuronType: additionalInputNeuronType,
                    additionalInputs: additionalInputs
                )
            )
            {
                result.AddRange(
                    [
                        half1_XOR___Addend1__Addend2,
                        half1_AND___Addend1__Addend2
                    ]
                );
            }
            return result;
        }
    }
}
