using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public interface ITruthTable
    {
    }

    public interface ITruthTable
    <
        TParam,
        TInterneuron
    > :
        ITruthTable,
        ICircuit
        <
            TParam,
            TInterneuron
        >
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
    }

    public interface ITruthTableStatic
    <
        T, 
        TParam, 
        TInterneuron
    > :
        ITruthTable
        <
            TParam,
            TInterneuron
        >
        where T : 
            ITruthTable
            <
                TParam, 
                TInterneuron
            >
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        static abstract bool TryCreate
        (
            [NotNullWhen(true)] out T? result,
            TParam parameters,
            InterneuronTagInfo? interneuronTagInfo = null,
            AdditionalInputNeuronInfo? additionalInputNeurons = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        );

        static abstract TInterneuron CreateInterneurons
        (
            TParam parameters,
            VariableInfo variableInfo,
            InterneuronTagInfo? interneuronTagInfo = null,
            AdditionalInputNeuronInfo? additionalInputNeurons = null
        );

        static abstract T Create
        (
            TParam parameters,
            TInterneuron interneurons,
            VariableInfo? variableInfo
        );
    }
}
