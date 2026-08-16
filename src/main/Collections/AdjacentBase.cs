using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Collections
{
    public abstract class AdjacentBase<TInput, TOutput>(
        FunctionalCircuitParameter<TInput, TOutput> parameters,
        InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) : 
        FunctionalCircuitBase<TInput, TOutput, InterneuronSet>(
            parameters,
            interneurons,
            variableInfo
        )
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            InterneuronSet? precedingInterneurons = default,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            params Neuron[] additionalInputs
        ) 
            where T : IAdjacent<T, TInput, TOutput, InterneuronSet>
        {
            bool bResult = false;
            result = default;

            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = T.Create(
                    parameters,
                    T.CreateInterneurons(
                        parameters,
                        variableInfo,
                        precedingInterneurons,
                        additionalInputs
                    ),
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }

        protected static ReadOnlyNetwork LinkInputNeurons(
            UnaryNeuronParameter current,
            ReadOnlyNetwork interneuron1,
            InterneuronSet? precedingInterneurons = default,
            params Neuron[] additionalInputs
        )
        {
            var inputNeurons = new List<NeuronInfo>([new(current.Neuron)]);
            if (precedingInterneurons != null)
                inputNeurons.Add(new NeuronInfo(precedingInterneurons.Interneuron1.GetInterneuron(), 1f, NeurotransmitterEffect.Inhibit));

            return NetworkHelper.LinkInputNeuronsToInterneuron(
                interneuron1.GetInterneuron(),
                [.. inputNeurons],
                additionalInputNeuronInfos: [..additionalInputs.Select(n => new NeuronInfo(n))]
            );
        }
    }
}
