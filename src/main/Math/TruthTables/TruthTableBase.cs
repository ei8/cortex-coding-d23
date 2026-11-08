using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public abstract class TruthTableBase
    <
        T,
        TParam,
        TInput,
        TOutput,
        TInterneuron,
        TInterneuronTagger
    >
    (
        TParam parameters,
        TInterneuron interneuron,
        VariableInfo? variableInfo
    ) :
        FunctionalCircuitBase
        <
            TParam,
            TInterneuron
        >
        (
            parameters,
            interneuron,
            variableInfo
        )
        where T :
            ITruthTableStatic
            <
                T,
                TParam,
                TInput,
                TOutput,
                TInterneuron,
                TInterneuronTagger
            >
        where TParam : IFunctionalCircuitParameter
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
        where TInterneuron : ICircuitInterneuronSet
        where TInterneuronTagger : IInterneuronTagger
    {
        public static bool TryCreate
        (
            [NotNullWhen(true)] out T? result,
            TParam parameters,
            TInterneuronTagger? interneuronTagger = default,
            AdditionalInputNeuronInfo? additionalInputNeurons = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
        {
            bool bResult = false;
            result = default;
            if(VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                var interneurons = T.CreateInterneurons
                (
                    parameters,
                    variableInfo,
                    interneuronTagger,
                    additionalInputNeurons
                );

                result = T.Create
                (
                    parameters,
                    interneurons,
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }
    }
}
