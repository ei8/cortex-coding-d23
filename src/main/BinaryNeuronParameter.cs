using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23
{
    public class BinaryNeuronParameter : NeuronParameterBase
    {
        private BinaryNeuronParameter(IEnumerable<NeuronInfo> neuronInfos) : base(neuronInfos)
        {
        }

        public static BinaryNeuronParameter Create(
            string tagPrefix,
            string trueString = "1",
            string falseString = "0"
        ) =>
            new(
                [
                    new(NetworkHelper.CreateNeuron($"{tagPrefix} = {trueString}")),
                    new(NetworkHelper.CreateNeuron($"{tagPrefix} = {falseString}"))
                ]
            );

        public static bool TryCreate(
            [NotNullWhen(true)] out BinaryNeuronParameter? result,
            string? tagPrefix = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            string trueString = "1",
            string falseString = "0"
        )
        {
            bool bResult = false;
            result = null;

            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = BinaryNeuronParameter.Create(
                    $"{tagPrefix}" +
                    (string.IsNullOrWhiteSpace(tagPrefix) ? string.Empty : ".") +
                    variableInfo.ToString(),
                    trueString,
                    falseString
                );
                result.VariableInfo = variableInfo;
                bResult = true;
            }

            return bResult;
        }

        public Neuron Neuron1 => this.NeuronInfos.First().Neuron;

        public Neuron Neuron0 => this.NeuronInfos.ElementAt(1).Neuron;
    }
}
