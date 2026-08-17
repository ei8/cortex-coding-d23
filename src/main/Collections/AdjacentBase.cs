using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Collections
{
    public abstract class AdjacentBase<TParam, TInterneuron>
    (
        TParam parameters,
        TInterneuron interneurons,
        VariableInfo? variableInfo
    ) : 
        FunctionalCircuitBase
        <
            TParam, 
            TInterneuron
        >
        (
            parameters,
            interneurons,
            variableInfo
        )
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        public static bool TryCreate<T>
        (
            [NotNullWhen(true)] out T? result,
            TParam parameters,
            TInterneuron? precedingInterneurons = default,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            params Neuron[] additionalInputs
        ) 
            where T : IAdjacent<T, TParam, TInterneuron>
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
    }
}
