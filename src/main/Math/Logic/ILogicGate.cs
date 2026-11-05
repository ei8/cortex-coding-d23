using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public interface ILogicGate
    {
    }

    public interface ILogicGate
    <
        TParam, 
        TInterneuron
    > :
        ILogicGate,
        ITruthTable
        <
            TParam, 
            TInterneuron
        >
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
    }

    public interface ILogicGateStatic
    <
        T,
        TParam,
        TInput,
        TOutput,
        TInterneuron
    > :
        ITruthTableStatic
        <
            T,
            TParam,
            TInterneuron
        >,
        ILogicGate
        <
            TParam,
            TInterneuron
        >
        where T :
            ILogicGate
            <
                TParam,
                TInterneuron
            >
        where TParam : IFunctionalCircuitParameter
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
        where TInterneuron : ICircuitInterneuronSet
    {
        static abstract IEnumerable<IEnumerable<Neuron>> GetOutputsPerInterneuron(TOutput outputs);

        static abstract IEnumerable<string> GetInterneuronTags
        (
            VariableInfo variableInfo,
            InterneuronTagInfo? interneuronTagInfo = null
        );

        static abstract IEnumerable<ReadOnlyNetwork> LinkInputNeurons
        (
            TInput inputs,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            AdditionalInputNeuronInfo? additionalInputNeurons
        );
    }
}
