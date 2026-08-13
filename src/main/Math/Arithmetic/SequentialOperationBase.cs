using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class SequentialOperationBase : FunctionalCircuitBase<NeuronParameterBase>
    {
        protected SequentialOperationBase(
            FunctionalParameter<NeuronParameterBase> parameters,
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
            FunctionalParameter<NeuronParameterBase> parameters,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        )
            where T : ISequentialOperation<T>
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
