using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class SequentialOperationBase<TParam, TInterneuron> : FunctionalCircuitBase<TParam, TInterneuron>
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        protected SequentialOperationBase(
            TParam parameters,
            TInterneuron interneurons,
            VariableInfo? variableInfo
        ) :
        base(
            parameters,
            interneurons,
            variableInfo
        )
        {
        }

        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? tryResult,
            TParam parameters,
            TInterneuron interneurons,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        )
            where T : ISequentialOperation<T, TParam, TInterneuron>
        {
            tryResult = T.Create(
                parameters,
                interneurons,
                variableInfo
            );

            return true;
        }
    }
}
