using System.Collections.Generic;

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
        static abstract IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output);

        static abstract IEnumerable<string> GetInterneuronTags
        (
            VariableInfo variableInfo,
            InterneuronTagInfo? interneuronTagInfo = null
        );

        static abstract T Create
        (
            TParam parameters,
            TInterneuron interneurons,
            VariableInfo? variableInfo
        );

        static abstract IEnumerable<ReadOnlyNetwork> LinkInputNeurons
        (
            TParam parameters,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            NetworkHelper.InputNeuronStrengthMode additionalInputNeuronType = NetworkHelper.InputNeuronStrengthMode.And,
            params Neuron[] additionalInputs
        );
    }
}
