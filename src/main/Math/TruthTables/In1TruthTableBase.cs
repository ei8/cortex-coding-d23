using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public abstract partial class In1TruthTableBase
    <
        T,
        TParam,
        TInput,
        TOutput
    >
    (
        TParam parameters,
        In1TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TruthTableBase
        <
            T,
            TParam,
            TInput,
            TOutput,
            In1TruthTableBase.InterneuronSet,
            StaticInterneuronTagger
        >
        (
            parameters,
            interneurons,
            variableInfo
        )
        where T :
            IInTruthTableStatic
            <
                T,
                TParam,
                TInput,
                TOutput,
                In1TruthTableBase.InterneuronSet,
                StaticInterneuronTagger
            >
        where TParam : IFunctionalCircuitParameter<TInput, TOutput>
        where TInput : IInputCircuitParameterSubset<BinaryNeuronParameter>
        where TOutput : IOutputCircuitParameterSubset
    {
        public static In1TruthTableBase.InterneuronSet CreateInterneurons
        (
            TParam parameters,
            VariableInfo variableInfo,
            StaticInterneuronTagger? interneuronTagger = null,
            AdditionalInputNeuronInfo? additionalInputNeurons = null
        )
        {
            var interneuronNetworks = NetworkHelper.CreateInterneuronNetworksByOutputNeurons
                (
                    T.GetOutputsPerInterneuron(parameters.Outputs),
                    T.GetInterneuronTags(variableInfo, interneuronTagger)
                );

            return new In1TruthTableBase.InterneuronSet
            (
                interneuronNetworks.ElementAt(0),
                interneuronNetworks.ElementAt(1),
                T.LinkInputNeurons(
                    parameters.Inputs,
                    interneuronNetworks,
                    additionalInputNeurons
                ).Combine()
            );
        }

        public static IEnumerable<string> GetInterneuronTags
        (
            VariableInfo variableInfo,
            StaticInterneuronTagger? interneuronTagger = null
        )
        {
            string coreOperatorPrefix = string.Empty,
                coreInputTagPrefix = string.Empty;

            if (interneuronTagger?.Value != null)
            {
                if (!string.IsNullOrEmpty(interneuronTagger.Value.TypeTagPrefix))
                    coreOperatorPrefix = $"{interneuronTagger.Value.TypeTagPrefix}.";
                if (interneuronTagger.Value.InputTagPrefixes?.Count() > 0)
                    coreInputTagPrefix = $"{interneuronTagger.Value.InputTagPrefixes.ElementAt(0)}.";
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
