using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class SequentialOperationBase<TInput, TOutput, TInterneuron> : FunctionalCircuitBase<TInput, TOutput, TInterneuron>
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
        where TInterneuron : ICircuitInterneuronSet
    {
        protected SequentialOperationBase(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
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
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            TInterneuron interneurons,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        )
            where T : ISequentialOperation<T, TInput, TOutput, TInterneuron>
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
