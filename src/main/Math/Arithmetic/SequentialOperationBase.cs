using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class SequentialOperationBase<TInput, TOutput> : FunctionalCircuitBase<TInput, TOutput>
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
        protected SequentialOperationBase(
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
            [NotNullWhen(true)] out T? tryResult,
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        )
            where T : ISequentialOperation<T, TInput, TOutput>
        {
            tryResult = T.Create(
                parameters,
                [
                    ..interneuronNetworks
                ],
                variableInfo
            );

            return true;
        }
    }
}
