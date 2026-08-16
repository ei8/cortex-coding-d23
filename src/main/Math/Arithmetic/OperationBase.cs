using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public abstract class OperationBase<TInput, TOutput>(
        FunctionalCircuitParameter<TInput, TOutput> parameters,
        InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) : FunctionalCircuitBase<TInput, TOutput, InterneuronSet>(
        parameters,
        interneurons,
        variableInfo
    )
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            int exponent = 0,
            BinaryNeuronParameter? precedingValue = null,
            VariableInfo? precedingVariableInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
            where T : IOperation<T, TInput, TOutput, InterneuronSet>
        {
            bool bResult = false;
            result = default;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                var parameters = T.GetDefaultParameters(
                    precedingValue,
                    exponent
                );
                result = T.Create(
                    parameters,
                    T.CreateInterneurons(
                        parameters,
                        variableInfo,
                        precedingVariableInfo
                    ),
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }
    }
}
