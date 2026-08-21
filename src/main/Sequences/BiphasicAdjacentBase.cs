using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Sequences
{
    public class BiphasicAdjacentBase<TParam, TInterneuron>
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
            float inputStrength = 0.5f,
            TInterneuron? precedingInterneurons = default,
            float interPhaseStrength = 1f,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            params NeuronInfo[] additionalInputs
        )
            where T : IBiphasicAdjacent<T, TParam, TInterneuron>
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
                        inputStrength,
                        precedingInterneurons,
                        interPhaseStrength,
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