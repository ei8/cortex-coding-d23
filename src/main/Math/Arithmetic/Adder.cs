using ei8.Cortex.Coding.d23.Math.Logic;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class Adder : OperationBase, IOperation<Adder>
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

        protected Adder(
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

        public static Adder Create(
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
                BinaryNeuronParameter.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Input.Addend1)}"),
                BinaryNeuronParameter.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Input.Addend2)}"),
                precedingValue
            ],
            [
                BinaryNeuronParameter.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Output.Sum)}"),
                BinaryNeuronParameter.Create($"{nameof(Adder)}{exponent + 1}.{nameof(Output.CarryOver)}")
            ]
        );

        public static IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworks(
            FunctionalParameter<BinaryNeuronParameter> parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        ) => Adder.CreateInterneuronNetworksCore(
            variableInfo,
            precedingVariableInfo,
            parameters.Inputs.ElementAt((int)Input.PrecedingCarryOver),
            parameters.Inputs.ElementAt((int)Input.Addend1),
            parameters.Inputs.ElementAt((int)Input.Addend2),
            parameters.Outputs.ElementAt((int)Output.Sum),
            parameters.Outputs.ElementAt((int)Output.CarryOver)
        );

        internal static IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworksCore(
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo,
            BinaryNeuronParameter? precedingCarryOver,
            BinaryNeuronParameter? addend1,
            BinaryNeuronParameter? addend2,
            BinaryNeuronParameter? sum,
            BinaryNeuronParameter? carryOver,
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
                        additionalInputs
                    )
                );

                // half2
                if (
                    DualInputLogicGateBase.TryCreate(
                        out XorGate? half2_XOR___CarryOver__Half1_XOR_Result,
                        new(
                            precedingCarryOver,
                            half1_XOR_Result,
                            sum
                        ),
                        new(
                            [
                                precedingAdderName,
                                adderName
                            ],
                            adderName
                        )
                    ) &&
                    DualInputLogicGateBase.TryCreate(
                        out AndGate? half2_AND___CarryOver__Half1_XOR_Result,
                        new(
                            precedingCarryOver,
                            half1_XOR_Result,
                            half2_CarryOver
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
                    DualInputLogicGateBase.TryCreate(
                        out OrGate? OR___Half1_CarryOver__Half2_CarryOver,
                        new(
                            half1_CarryOver,
                            half2_CarryOver,
                            carryOver
                        ),
                        new DualInputInterneuronTagInfo(adderName)
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
                        additionalInputs
                    )
                );
            }

            return result.Select(n => n.Network);
        }

        private static IEnumerable<IneurUL> CreateAdderHalf1Interneurons(
            BinaryNeuronParameter? addend1,
            BinaryNeuronParameter? addend2,
            BinaryNeuronParameter? xorOutput,
            BinaryNeuronParameter? andOutput,
            string prefix,
            params Neuron[] additionalInputs
        )
        {
            var result = new List<IneurUL>();
            // Link half1 interneurons
            if (
                DualInputLogicGateBase.TryCreate(
                    out XorGate? half1_XOR___Addend1__Addend2,
                    new(
                        addend1,
                        addend2,
                        xorOutput
                    ),
                    new DualInputInterneuronTagInfo(prefix),
                    additionalInputs: additionalInputs
                ) &&
                DualInputLogicGateBase.TryCreate(
                    out AndGate? half1_AND___Addend1__Addend2,
                    new(
                        addend1,
                        addend2,
                        andOutput
                    ),
                    new DualInputInterneuronTagInfo(prefix),
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
