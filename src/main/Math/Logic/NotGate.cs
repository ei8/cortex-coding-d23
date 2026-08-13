using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NotGate : LogicGateBase, ISingleInputLogicGate<NotGate>
    {
        protected NotGate(
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

        public static NotGate Create(
            FunctionalParameter<BinaryNeuronParameter> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) => new(
            parameters,
            networks,
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
            BinaryNeuronParameter input, 
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            params Neuron[] additionalInputs
        )
        {
            var result = new List<ReadOnlyNetwork>();
            result.AddRange(
                [
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(0).GetInterneuron(),
                        [
                            new(input.Neuron0),
                            .. additionalInputs.Select(n => new NeuronInfo(n))
                        ]
                    ),
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(1).GetInterneuron(),
                        [
                            new(input.Neuron1),
                            .. additionalInputs.Select(n => new NeuronInfo(n))
                        ]
                    )
                ]
            );
            return result;
        }

        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            SingleInputFunctionalParameter<BinaryNeuronParameter> parameters,
            InterneuronTagInfo? interneuronTagInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            params Neuron[] additionalInputs
        )
            where T : ISingleInputLogicGate<T>
        {
            bool bResult = false;
            result = default;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                var output = parameters.Output;
                var interneuronNetworks = Enumerable.Empty<ReadOnlyNetwork>();
                if (output != null)
                    interneuronNetworks = NetworkHelper.CreateInterneuronNetworksByOutputNeurons(
                        T.GetInterneuronOutputs(output),
                        T.GetInterneuronTags(variableInfo, interneuronTagInfo)
                    );

                if (parameters.Input != null)
                {
                    result = T.Create(
                        parameters,
                        [
                            ..interneuronNetworks,
                        ..T.LinkInputNeurons(
                            parameters.Input,
                            interneuronNetworks,
                            additionalInputs
                        )
                        ],
                        variableInfo
                    );
                    bResult = true;
                }
            }

            return bResult;
        }
    }
}
