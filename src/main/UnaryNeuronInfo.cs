
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23
{
    public class UnaryNeuronInfo : NeuronInfoBase
    {
        public UnaryNeuronInfo(Neuron neuron) : base()
        {
            this.Neuron = neuron;

            this.network.AddReplaceItems(
                [
                    this.Neuron
                ]
            );
        }

        public static UnaryNeuronInfo Create(string tag) =>
            new(NetworkHelper.CreateNeuron(tag));

        public static bool TryCreate(
            [NotNullWhen(true)] out UnaryNeuronInfo? result,
            string? tagPrefix = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
        {
            bool bResult = false;
            result = null;

            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = UnaryNeuronInfo.Create(
                    $"{tagPrefix}" +
                    (string.IsNullOrWhiteSpace(tagPrefix) ? string.Empty : ".") +
                    variableInfo.ToString()
                );
                bResult = true;
            }

            return bResult;
        }

        public Neuron Neuron { get; }
    }
}
