using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public abstract class OperationBase<TParam, TInterneuron>(
        TParam parameters,
        TInterneuron interneurons,
        VariableInfo? variableInfo
    ) : FunctionalCircuitBase<TParam, TInterneuron>(
        parameters,
        interneurons,
        variableInfo
    )
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            int exponent = 0,
            BinaryNeuronParameter? precedingValue = null,
            VariableInfo? precedingVariableInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
            where T : IOperation<T, TParam, TInterneuron>
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
