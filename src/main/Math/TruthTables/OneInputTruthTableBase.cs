using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public abstract partial class OneInputTruthTableBase
    (
        OneInputTruthTableBase.ParameterInfo parameters,
        OneInputTruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TruthTableBase
        <
            OneInputTruthTableBase.ParameterInfo,
            OneInputTruthTableBase.InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        )
    {
        public static IEnumerable<string> GetInterneuronTags
        (
            VariableInfo variableInfo, 
            InterneuronTagInfo? interneuronTagInfo = null
        )
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

        public static IEnumerable<ReadOnlyNetwork> LinkInputNeurons
        (
            OneInputTruthTableBase.ParameterInfo parameters,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            NetworkHelper.InputNeuronStrengthMode additionalInputNeuronType = NetworkHelper.InputNeuronStrengthMode.And,
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

        public static bool TryCreate<T>
        (
            [NotNullWhen(true)] out T? result,
            OneInputTruthTableBase.ParameterInfo parameters,
            InterneuronTagInfo? interneuronTagInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            NetworkHelper.InputNeuronStrengthMode additionalInputNeuronType = NetworkHelper.InputNeuronStrengthMode.And,
            params Neuron[] additionalInputs
        )
            where T :
                ITruthTableStatic
                <
                    T,
                    OneInputTruthTableBase.ParameterInfo,
                    OneInputTruthTableBase.InterneuronSet
                >
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

                    var interneurons = new OneInputTruthTableBase.InterneuronSet
                    (
                        interneuronNetworks.ElementAt(0),
                        interneuronNetworks.ElementAt(1),
                        T.LinkInputNeurons(
                            parameters,
                            interneuronNetworks,
                            additionalInputNeuronType,
                            additionalInputs
                        ).Combine()
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
