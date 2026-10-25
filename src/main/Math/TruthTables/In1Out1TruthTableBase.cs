using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public abstract partial class In1Out1TruthTableBase
    (
        In1Out1TruthTableBase.ParameterInfo parameters,
        In1Out1TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TruthTableBase
        <
            In1Out1TruthTableBase.ParameterInfo,
            In1Out1TruthTableBase.InterneuronSet
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
            In1Out1TruthTableBase.Input inputs,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            NetworkHelper.InputNeuronStrengthMode additionalInputNeuronType = NetworkHelper.InputNeuronStrengthMode.And,
            params Neuron[] additionalInputs
        )
        {
            var result = new List<ReadOnlyNetwork>();

            if (inputs.Input1 != null)
            {
                result.AddRange(
                    [
                        NetworkHelper.LinkInputNeuronsToInterneuron(
                            interneuronNetworks.ElementAt(0).GetInterneuron(),
                            [
                                new(inputs.Input1.Neuron0),
                            ],
                            additionalInputNeuronType,
                            [
                                ..additionalInputs.Select(n => new NeuronInfo(n))
                            ]
                        ),
                        NetworkHelper.LinkInputNeuronsToInterneuron(
                            interneuronNetworks.ElementAt(1).GetInterneuron(),
                            [
                                new(inputs.Input1.Neuron1),
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
            In1Out1TruthTableBase.ParameterInfo parameters,
            InterneuronTagInfo? interneuronTagInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            NetworkHelper.InputNeuronStrengthMode additionalInputNeuronType = NetworkHelper.InputNeuronStrengthMode.And,
            params Neuron[] additionalInputs
        )
            where T :
                ITruthTableStatic
                <
                    T,
                    In1Out1TruthTableBase.ParameterInfo,
                    In1Out1TruthTableBase.Input,
                    In1Out1TruthTableBase.Output,
                    In1Out1TruthTableBase.InterneuronSet
                >
        {
            bool bResult = false;
            result = default;
            if 
            (
                VariableInfo.TryParse(parameterExpression, out var variableInfo) &&
                parameters.Outputs.Output1 != null &&
                parameters.Inputs.Input1 != null
            )
            {
                var interneuronNetworks = NetworkHelper.CreateInterneuronNetworksByOutputNeurons
                (
                    T.GetOutputsPerInterneuron(parameters.Outputs),
                    T.GetInterneuronTags(variableInfo, interneuronTagInfo)
                );

                var interneurons = new In1Out1TruthTableBase.InterneuronSet
                (
                    interneuronNetworks.ElementAt(0),
                    interneuronNetworks.ElementAt(1),
                    T.LinkInputNeurons(
                        parameters.Inputs,
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

            return bResult;
        }
    }
}
