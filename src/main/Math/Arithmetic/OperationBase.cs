using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public abstract class OperationBase<TInput, TOutput> : FunctionalCircuitBase<TInput, TOutput>
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
        protected OperationBase(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) : 
        base(
            parameters,
            networks,
            variableInfo
        )
        {
        }

        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            int exponent = 0,
            BinaryNeuronParameter? precedingValue = null,
            VariableInfo? precedingVariableInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
            where T : IOperation<T, TInput, TOutput>
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
                    [
                        ..T.CreateInterneuronNetworks(
                            parameters,
                            variableInfo,
                            precedingVariableInfo
                        )
                    ],
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }
    }
}
