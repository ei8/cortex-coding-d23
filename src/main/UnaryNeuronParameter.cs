using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23
{
    public class UnaryNeuronParameter
    (
        IEnumerable<NeuronInfo> neuronInfos, 
        VariableInfo? variableInfo
    ) : 
        NeuronParameterBase
        (
            neuronInfos, 
            variableInfo
        )
    {
        public static bool TryCreate(
            [NotNullWhen(true)] out UnaryNeuronParameter? result,
            string? tagPrefix = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
        {
            bool bResult = false;
            result = null;

            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = new
                (
                    [
                        new
                        (
                            NetworkHelper.CreateNeuron
                            (
                                $"{tagPrefix}" +
                                (string.IsNullOrWhiteSpace(tagPrefix) ? string.Empty : ".") +
                                variableInfo.ToString()
                            )
                        )
                    ],
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }

        public Neuron Neuron => this.NeuronInfos.Single().Neuron;
    }
}
