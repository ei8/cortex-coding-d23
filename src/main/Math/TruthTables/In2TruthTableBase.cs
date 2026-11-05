using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    // TODO: Change references to BinaryNeuronParameter to generic TNeuronParam
    // TODO: Create BinaryTwoInputTruthTableBase and 1) specify BinaryNeuronParameter; 2) Transfer LinkInputNeurons and GetInterneuronTags there
    public abstract partial class In2TruthTableBase
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
        where T : ITruthTableStatic
            <
                T,
                TParam,
                TInterneuron
            >
        where TParam : IFunctionalCircuitParameter<TInput>
        where TInput : 
            IInputCircuitParameterSubset
            <
                BinaryNeuronParameter,
                BinaryNeuronParameter
            >
        where TInterneuron : ICircuitInterneuronSet
    {
        public static IEnumerable<string> GetInterneuronTags
        (
            VariableInfo variableInfo, 
            InterneuronTagInfo? interneuronTagInfo = null
        )
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

        public static IEnumerable<ReadOnlyNetwork> LinkInputNeurons
        (
            TInput inputs,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            AdditionalInputNeuronInfo? additionalInputNeurons
        )
        {
            var result = new List<ReadOnlyNetwork>();

            if (inputs.Parameter1 != null && inputs.Parameter2 != null)
            {
                result.AddRange(
                    [
                        NetworkHelper.LinkInputNeuronsToInterneuron(
                            interneuronNetworks.ElementAt(0).GetInterneuron(),
                            [
                                new(inputs.Parameter1.Neuron0),
                                new(inputs.Parameter2.Neuron0)
                            ],
                            additionalInputNeurons
                        ),
                        NetworkHelper.LinkInputNeuronsToInterneuron(
                            interneuronNetworks.ElementAt(1).GetInterneuron(),
                            [
                                new(inputs.Parameter1.Neuron0),
                                new(inputs.Parameter2.Neuron1),
                            ],
                            additionalInputNeurons
                        ),
                        NetworkHelper.LinkInputNeuronsToInterneuron(
                            interneuronNetworks.ElementAt(2).GetInterneuron(),
                            [
                                new(inputs.Parameter1.Neuron1),
                                new(inputs.Parameter2.Neuron0),
                            ],
                            additionalInputNeurons
                        ),
                        NetworkHelper.LinkInputNeuronsToInterneuron(
                            interneuronNetworks.ElementAt(3).GetInterneuron(),
                            [
                                new(inputs.Parameter1.Neuron1),
                                new(inputs.Parameter2.Neuron1),
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
