using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public abstract partial class In1TruthTableBase
    <
        T,
        TParam,
        TInput,
        TInterneuron
    >
    (
        TParam parameters,
        TInterneuron interneurons,
        VariableInfo? variableInfo
    ) :
        TruthTableBase
        <
            T,
            TParam,
            TInterneuron
        >
        (
            parameters,
            interneurons,
            variableInfo
        )
        where T :
            ITruthTableStatic
            <
                T,
                TParam,
                TInterneuron
            >
        where TParam : IFunctionalCircuitParameter<TInput>
        where TInput : IInputCircuitParameterSubset<BinaryNeuronParameter>
        where TInterneuron : ICircuitInterneuronSet
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
            TInput inputs,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            AdditionalInputNeuronInfo? additionalInputNeurons
        )
        {
            var result = new List<ReadOnlyNetwork>();

            if (inputs.Parameter1 != null)
            {
                result.AddRange(
                    [
                        NetworkHelper.LinkInputNeuronsToInterneuron(
                            interneuronNetworks.ElementAt(0).GetInterneuron(),
                            [
                                new(inputs.Parameter1.Neuron0),
                            ],
                            additionalInputNeurons
                        ),
                        NetworkHelper.LinkInputNeuronsToInterneuron(
                            interneuronNetworks.ElementAt(1).GetInterneuron(),
                            [
                                new(inputs.Parameter1.Neuron1),
                            ],
                            additionalInputNeurons
                        )
                    ]
                );
            }
            return result;
        }
    }
}
