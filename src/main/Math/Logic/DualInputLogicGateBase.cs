using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class DualInputLogicGateBase : LogicGateBase
    {
        protected DualInputLogicGateBase() : base()
        {
        }

        protected override IEnumerable<string> GetInterneuronTags(VariableInfo variableInfo, InterneuronTagInfo? interneuronTagInfo = null)
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

        protected override IEnumerable<ReadOnlyNetwork> LinkInputNeurons(
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
                            inputs.ElementAt(0).Neuron0,
                            inputs.ElementAt(1).Neuron0,
                            .. additionalInputs
                        ]
                    ),
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(1).GetInterneuron(),
                        [
                            inputs.ElementAt(0).Neuron0,
                            inputs.ElementAt(1).Neuron1,
                            .. additionalInputs
                        ]
                    ),
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(2).GetInterneuron(),
                        [
                            inputs.ElementAt(0).Neuron1,
                            inputs.ElementAt(1).Neuron0,
                            .. additionalInputs
                        ]
                    ),
                    NetworkHelper.LinkInputNeuronsToInterneuron(
                        interneuronNetworks.ElementAt(3).GetInterneuron(),
                        [
                            inputs.ElementAt(0).Neuron1,
                            inputs.ElementAt(1).Neuron1,
                            .. additionalInputs
                        ]
                    )
                ]
            );

            return result;
        }
    }
}
