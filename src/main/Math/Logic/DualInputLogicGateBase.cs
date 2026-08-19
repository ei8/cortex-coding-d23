using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class DualInputLogicGateBase
    (
        FunctionalCircuitParameter
        <
            DualInputLogicGateBase.Input, 
            DualInputLogicGateBase.Output
        >
        parameters,
        DualInputLogicGateBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) : 
        LogicGateBase
        <
            FunctionalCircuitParameter
            <
                DualInputLogicGateBase.Input, 
                DualInputLogicGateBase.Output
            >,
            DualInputLogicGateBase.InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        )
    {
        public class Input(
            BinaryNeuronParameter? input1,
            BinaryNeuronParameter? input2
        ) :
        InputCircuitParameterSubset<BinaryNeuronParameter, BinaryNeuronParameter>(
            input1,
            input2
        )
        {
            public BinaryNeuronParameter? Input1 => this.Parameter1;
            public BinaryNeuronParameter? Input2 => this.Parameter2;
        }

        public class Output(
            BinaryNeuronParameter? output1
        ) :
        OutputCircuitParameterSubset<BinaryNeuronParameter>(
            output1
        )
        {
            public BinaryNeuronParameter? Output1 => this.Parameter1;
        }

        public class InterneuronSet
        (
            ReadOnlyNetwork interneuron1,
            ReadOnlyNetwork interneuron2,
            ReadOnlyNetwork interneuron3,
            ReadOnlyNetwork interneuron4,
            ReadOnlyNetwork linkedInputNeurons
        ) :
            CircuitInterneuronSetBase,
            ICircuitInterneuronSet
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            [
                this.Interneuron1,
                this.Interneuron2,
                this.Interneuron3,
                this.Interneuron4,
                this.LinkedInputNeurons
            ];

            public ReadOnlyNetwork Interneuron1 = interneuron1;
            public ReadOnlyNetwork Interneuron2 = interneuron2;
            public ReadOnlyNetwork Interneuron3 = interneuron3;
            public ReadOnlyNetwork Interneuron4 = interneuron4;
            public ReadOnlyNetwork LinkedInputNeurons = linkedInputNeurons;
        }

        public static IEnumerable<string> GetInterneuronTags(VariableInfo variableInfo, InterneuronTagInfo? interneuronTagInfo = null)
        {
            string typeTagPrefix = string.Empty,
                input1TagPrefix = string.Empty,
                input2TagPrefix = string.Empty;

            if (interneuronTagInfo != null)
            {
                typeTagPrefix = $"{interneuronTagInfo.TypeTagPrefix}.";
                input1TagPrefix = $"{interneuronTagInfo.InputTagPrefixes.ElementAt(0)}.";
                input2TagPrefix = $"{interneuronTagInfo.InputTagPrefixes.ElementAt(1)}.";
            }

            return [
                $"{typeTagPrefix}{variableInfo.Function}({input1TagPrefix}{variableInfo.Inputs.First()} = 0," +
                $"{input2TagPrefix}{variableInfo.Inputs.ElementAt(1)} = 0)",
                $"{typeTagPrefix}{variableInfo.Function}({input1TagPrefix}{variableInfo.Inputs.First()} = 0," +
                $"{input2TagPrefix}{variableInfo.Inputs.ElementAt(1)} = 1)",
                $"{typeTagPrefix}{variableInfo.Function}({input1TagPrefix}{variableInfo.Inputs.First()} = 1," +
                $"{input2TagPrefix}{variableInfo.Inputs.ElementAt(1)} = 0)",
                $"{typeTagPrefix}{variableInfo.Function}({input1TagPrefix}{variableInfo.Inputs.First()} = 1," +
                $"{input2TagPrefix}{variableInfo.Inputs.ElementAt(1)} = 1)",
            ];
        }

        public static IEnumerable<ReadOnlyNetwork> LinkInputNeurons(
            FunctionalCircuitParameter<DualInputLogicGateBase.Input, DualInputLogicGateBase.Output> parameters,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            NetworkHelper.InputNeuronStrengthMode additionalInputNeuronType = NetworkHelper.InputNeuronStrengthMode.And,
            params Neuron[] additionalInputs
        )
        {
            var result = new List<ReadOnlyNetwork>();

            if (parameters.Inputs.Parameter1 != null && parameters.Inputs.Parameter2 != null)
            {
                result.AddRange(
                    [
                        NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(0).GetInterneuron(),
                        [
                            new(parameters.Inputs.Parameter1.Neuron0),
                            new(parameters.Inputs.Parameter2.Neuron0)
                        ],
                        additionalInputNeuronType,
                        [
                            ..additionalInputs.Select(n => new NeuronInfo(n))
                        ]
                    ),
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(1).GetInterneuron(),
                        [
                            new(parameters.Inputs.Parameter1.Neuron0),
                            new(parameters.Inputs.Parameter2.Neuron1),
                        ],
                        additionalInputNeuronType,
                        [
                            ..additionalInputs.Select(n => new NeuronInfo(n))
                        ]
                    ),
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(2).GetInterneuron(),
                        [
                            new(parameters.Inputs.Parameter1.Neuron1),
                            new(parameters.Inputs.Parameter2.Neuron0),
                        ],
                        additionalInputNeuronType,
                        [
                            ..additionalInputs.Select(n => new NeuronInfo(n))
                        ]
                    ),
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(3).GetInterneuron(),
                        [
                            new(parameters.Inputs.Parameter1.Neuron1),
                            new(parameters.Inputs.Parameter2.Neuron1),
                        ],
                        additionalInputNeuronType,
                        [
                            ..additionalInputs.Select(n => new NeuronInfo(n))
                        ]
                    )
                    ]
                );
            }

            return result;
        }

        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            FunctionalCircuitParameter<DualInputLogicGateBase.Input, DualInputLogicGateBase.Output> parameters,
            InterneuronTagInfo? interneuronTagInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            NetworkHelper.InputNeuronStrengthMode additionalInputNeuronType = NetworkHelper.InputNeuronStrengthMode.And,
            params Neuron[] additionalInputs
        )
            where T : ILogicGate<T, FunctionalCircuitParameter<DualInputLogicGateBase.Input, DualInputLogicGateBase.Output>, InterneuronSet>
        {
            bool bResult = false;
            result = default;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                if 
                (
                    parameters.Outputs.Output1 != null && 
                    parameters.Inputs.Input1 != null && 
                    parameters.Inputs.Input2 != null
                )
                {
                    var interneuronNetworks = NetworkHelper.CreateInterneuronNetworksByOutputNeurons
                    (
                        T.GetInterneuronOutputs(parameters.Outputs.Output1),
                        T.GetInterneuronTags(variableInfo, interneuronTagInfo)
                    );
                    var interneurons = new DualInputLogicGateBase.InterneuronSet
                    (
                        interneuronNetworks.ElementAt(0),
                        interneuronNetworks.ElementAt(1),
                        interneuronNetworks.ElementAt(2),
                        interneuronNetworks.ElementAt(3),
                        T.LinkInputNeurons
                        (
                            parameters,
                            interneuronNetworks,
                            additionalInputNeuronType,
                            additionalInputs
                        ).Combine()
                    );

                    result = T.Create
                    (
                        parameters,
                        interneurons,
                        variableInfo
                    );
                    bResult = true;
                }
            }

            return bResult;
        }
    }
}
