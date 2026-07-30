using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public abstract class OperationBase : FunctionalCircuitBase<BinaryNeuronInfo>
    {
        public OperationBase() : base()
        {
        }

        protected abstract FunctionalParameter<BinaryNeuronInfo> GetDefaultParameters(
            BinaryNeuronInfo? precedingValue,
            int exponent
        );

        protected abstract IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworks(
            FunctionalParameter<BinaryNeuronInfo> parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        );

        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            int exponent = 0,
            BinaryNeuronInfo? precedingValue = null,
            VariableInfo? precedingVariableInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
            where T : OperationBase, new()
        {
            bool bResult = false;
            result = null;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = new();
                var parameters = result.GetDefaultParameters(
                    precedingValue,
                    exponent
                );
                result.Initialize(
                    parameters,
                    [
                        ..result.CreateInterneuronNetworks(
                            parameters,
                            variableInfo,
                            precedingVariableInfo
                        )
                    ]
                );
                result.VariableInfo = variableInfo;
                bResult = true;
            }

            return bResult;
        }
    }
}
