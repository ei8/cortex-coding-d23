using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public abstract class UngroupedOperationBase
    <
        TParam, 
        TInterneuron
    >
    (
        TParam parameters,
        TInterneuron interneurons,
        VariableInfo? variableInfo
    ) : 
        FunctionalCircuitBase<TParam, TInterneuron>
        (
            parameters,
            interneurons,
            variableInfo
        ),
        IUngroupedOperation
        <
            TParam, 
            TInterneuron
        >
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        public static bool TryCreate<T>
        (
            [NotNullWhen(true)] out T? result,
            int exponent = 0,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
            where T : 
                IUngroupedOperationStatic
                <
                    T, 
                    TParam, 
                    TInterneuron
                >, 
                IOperationStatic
                <
                    T, 
                    TParam, 
                    TInterneuron
                >
        {
            bool bResult = false;
            result = default;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                var parameters = T.GetDefaultParameters(exponent);
                result = T.Create(
                    parameters,
                    T.CreateInterneurons(
                        parameters,
                        variableInfo
                    ),
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }
    }
}
