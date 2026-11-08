using System.Collections.Generic;
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
        TInput,
        TOutput,
        TInterneuron,
        TInterneuronTagger
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
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
        where TInterneuron : ICircuitInterneuronSet
        where TInterneuronTagger : IInterneuronTagger
    {
        static abstract bool TryCreate
        (
            [NotNullWhen(true)] out T? result,
            TParam parameters,
            TInterneuronTagger? interneuronTagger = default,
            AdditionalInputNeuronInfo? additionalInputNeurons = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        );

        static abstract TInterneuron CreateInterneurons
        (
            TParam parameters,
            VariableInfo variableInfo,
            TInterneuronTagger? interneuronTagger = default,
            AdditionalInputNeuronInfo? additionalInputNeurons = null
        );

        static abstract T Create
        (
            TParam parameters,
            TInterneuron interneurons,
            VariableInfo? variableInfo
        );
    }

    public interface IInTruthTableStatic
    <
        T,
        TParam,
        TInput,
        TOutput,
        TInterneuron,
        TInterneuronTagger
    > :
        ITruthTableStatic
        <
            T,
            TParam,
            TInput,
            TOutput,
            TInterneuron,
            TInterneuronTagger
        >
        where T :
            ITruthTable
            <
                TParam,
                TInterneuron
            >
        where TParam : IFunctionalCircuitParameter
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
        where TInterneuron : ICircuitInterneuronSet
        where TInterneuronTagger : IInterneuronTagger
    {
        static abstract IEnumerable<IEnumerable<Neuron>> GetOutputsPerInterneuron(TOutput outputs);

        static abstract IEnumerable<string> GetInterneuronTags
        (
            VariableInfo variableInfo,
            TInterneuronTagger? tagger = default
        );

        static abstract IEnumerable<ReadOnlyNetwork> LinkInputNeurons
        (
            TInput inputs,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            AdditionalInputNeuronInfo? additionalInputNeurons
        );
    }
}
