using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NotGate : 
        LogicGateBase
        <
            NotGate.Input, 
            NotGate.Output, 
            NotGate.InterneuronSet
        >, 
        ILogicGate
        <
            NotGate, 
            NotGate.Input, 
            NotGate.Output, 
            NotGate.InterneuronSet
        >
    {
        public class Input(
            BinaryNeuronParameter? input1
        ) :
        InputCircuitParameterSubset<BinaryNeuronParameter>(
            input1
        )
        {
            public BinaryNeuronParameter? Input1 => this.Parameter1;
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
            ReadOnlyNetwork linkedInputNeurons
        ) :
            CircuitInterneuronSetBase,
            ICircuitInterneuronSet
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            [
                this.Interneuron1,
                this.Interneuron2,
                this.LinkedInputNeurons
            ];

            public ReadOnlyNetwork Interneuron1 = interneuron1;
            public ReadOnlyNetwork Interneuron2 = interneuron2;
            public ReadOnlyNetwork LinkedInputNeurons = linkedInputNeurons;
        }

        protected NotGate
        (
            FunctionalCircuitParameter<NotGate.Input, NotGate.Output> parameters,
            NotGate.InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) : 
            base
            (
                parameters,
                interneurons,
                variableInfo
            )
        {
        }

        public static NotGate Create(
            FunctionalCircuitParameter<NotGate.Input, NotGate.Output> parameters,
            NotGate.InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => new(
            parameters,
            interneurons,
            variableInfo
        );

        public static IEnumerable<string> GetInterneuronTags(VariableInfo variableInfo, InterneuronTagInfo? interneuronTagInfo = null)
        {
            string coreOperatorPrefix = string.Empty,
                coreInputTagPrefix = string.Empty;

            if (interneuronTagInfo != null)
            {
                if (!string.IsNullOrEmpty(interneuronTagInfo.TypeTagPrefix))
                    coreOperatorPrefix = $"{interneuronTagInfo.TypeTagPrefix}.";
                if (interneuronTagInfo.InputTagPrefixes?.Count() > 0)
                    coreInputTagPrefix = $"{interneuronTagInfo.InputTagPrefixes.ElementAt(0)}.";
            }

            return [
                $"{coreOperatorPrefix}{variableInfo.Function}({coreInputTagPrefix}{variableInfo.Inputs.Single()} = 0)",
                $"{coreOperatorPrefix}{variableInfo.Function}({coreInputTagPrefix}{variableInfo.Inputs.Single()} = 1)"
            ];
        }

        public static IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output) =>
        [
            output.Neuron1,
            output.Neuron0
        ];

        public static IEnumerable<ReadOnlyNetwork> LinkInputNeurons(
            FunctionalCircuitParameter<NotGate.Input, NotGate.Output> parameters, 
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            NetworkHelper.AdditionalInputNeuronType additionalInputNeuronType = NetworkHelper.AdditionalInputNeuronType.And,
            params Neuron[] additionalInputs
        )
        {
            var result = new List<ReadOnlyNetwork>();

            if (parameters.Inputs.Input1 != null)
            {
                result.AddRange(
                    [
                        NetworkHelper.LinkInputNeuronsToInterneuron(
                            interneuronNetworks.ElementAt(0).GetInterneuron(),
                            [
                                new(parameters.Inputs.Input1.Neuron0),
                            ],
                            additionalInputNeuronType,
                            [
                                ..additionalInputs.Select(n => new NeuronInfo(n))
                            ]
                        ),
                        NetworkHelper.LinkInputNeuronsToInterneuron(
                            interneuronNetworks.ElementAt(1).GetInterneuron(),
                            [
                                new(parameters.Inputs.Input1.Neuron1),
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
            FunctionalCircuitParameter<NotGate.Input, NotGate.Output> parameters,
            InterneuronTagInfo? interneuronTagInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            NetworkHelper.AdditionalInputNeuronType additionalInputNeuronType = NetworkHelper.AdditionalInputNeuronType.And,
            params Neuron[] additionalInputs
        )
            where T : ILogicGate<T, NotGate.Input, NotGate.Output, NotGate.InterneuronSet>
        {
            bool bResult = false;
            result = default;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                if 
                (
                    parameters.Outputs.Output1 != null &&
                    parameters.Inputs.Input1 != null
                )
                {
                    var interneuronNetworks = NetworkHelper.CreateInterneuronNetworksByOutputNeurons(
                        T.GetInterneuronOutputs(parameters.Outputs.Output1),
                        T.GetInterneuronTags(variableInfo, interneuronTagInfo)
                    );

                    var interneurons = new NotGate.InterneuronSet
                    (
                        interneuronNetworks.ElementAt(0),
                        interneuronNetworks.ElementAt(1),
                        T.LinkInputNeurons(
                            parameters,
                            interneuronNetworks,
                            additionalInputNeuronType,
                            additionalInputs
                        ).FromNetworks()
                    );
                
                    result = T.Create(
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
