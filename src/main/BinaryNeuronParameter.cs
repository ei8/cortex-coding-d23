using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23
{
    public class BinaryNeuronParameter : NeuronParameterBase
    {
        // TODO: Change to IEnumerable of Neurons then create enum for individual neurons
        // create terminal info?
        // create tag differentiator? eg. "1", "0"
        // create class/struct for 3 properties above (NeuronInfo)
        public BinaryNeuronParameter(Neuron neuron1, Neuron neuron0) : base()
        {
            this.Neuron1 = neuron1;
            this.Neuron0 = neuron0;

            this.network.AddReplaceItems(
                [
                    this.Neuron1,
                    this.Neuron0
                ]
            );
        }

        public static BinaryNeuronParameter Create(
            string tagPrefix,
            string trueString = "1",
            string falseString = "0"
        ) =>
            new(
                NetworkHelper.CreateNeuron($"{tagPrefix} = {trueString}"),
                NetworkHelper.CreateNeuron($"{tagPrefix} = {falseString}")
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

        public Neuron Neuron1 { get; }
        public Neuron Neuron0 { get; }
    }
}
