using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public abstract class OperationBase : FunctionalCircuitBase<BinaryNeuronParameter>
    {
        protected OperationBase(
            FunctionalParameter<BinaryNeuronParameter> parameters,
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
            where T : IOperation<T>
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
