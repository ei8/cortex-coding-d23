using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Collections
{
    public interface IAdjacent<T> : ICircuit<FunctionalParameter<UnaryNeuronParameter>, UnaryNeuronParameter>
        where T : ICircuit<FunctionalParameter<UnaryNeuronParameter>, UnaryNeuronParameter>
    {
        static abstract ReadOnlyNetwork CreateInterneuronNetwork(
            FunctionalParameter<UnaryNeuronParameter> parameters,
            VariableInfo variableInfo
        );

        static abstract ReadOnlyNetwork LinkInputNeurons(
            ReadOnlyNetwork interneuronNetwork,
            FunctionalParameter<UnaryNeuronParameter> parameters,
            ReadOnlyNetwork? precedingInterneuronNetwork = null,
            params Neuron[] additionalInputs
        );

        static abstract T Create(
            FunctionalParameter<UnaryNeuronParameter> parameters,
            ReadOnlyNetwork interneuronNetwork,
            ReadOnlyNetwork linkedInputNeurons,
            VariableInfo? variableInfo
        );
    }
}
