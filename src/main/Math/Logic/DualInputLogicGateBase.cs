using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class DualInputLogicGateBase : LogicGateBase
    {
        protected DualInputLogicGateBase(
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
            IEnumerable<BinaryNeuronParameter> inputs,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            params Neuron[] additionalInputs
        )
        {
            AssertionConcern.AssertArgumentValid(l => l == 2, inputs.Count(), "Length of inputs array must be exactly two.", nameof(inputs));

            var result = new List<ReadOnlyNetwork>();

            result.AddRange(
                [
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(0).GetInterneuron(),
                        [
                            new(inputs.ElementAt(0).Neuron0),
                            new(inputs.ElementAt(1).Neuron0),
                            .. additionalInputs.Select(n => new NeuronInfo(n))
                        ]
                    ),
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(1).GetInterneuron(),
                        [
                            new(inputs.ElementAt(0).Neuron0),
                            new(inputs.ElementAt(1).Neuron1),
                            .. additionalInputs.Select(n => new NeuronInfo(n))
                        ]
                    ),
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(2).GetInterneuron(),
                        [
                            new(inputs.ElementAt(0).Neuron1),
                            new(inputs.ElementAt(1).Neuron0),
                            .. additionalInputs.Select(n => new NeuronInfo(n))
                        ]
                    ),
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(3).GetInterneuron(),
                        [
                            new(inputs.ElementAt(0).Neuron1),
                            new(inputs.ElementAt(1).Neuron1),
                            .. additionalInputs.Select(n => new NeuronInfo(n))
                        ]
                    )
                ]
            );

            return result;
        }
    }
}
