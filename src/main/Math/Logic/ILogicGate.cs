using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public interface ILogicGate<T> : ICircuit<FunctionalParameter<BinaryNeuronParameter>, BinaryNeuronParameter>
        where T : ICircuit<FunctionalParameter<BinaryNeuronParameter>, BinaryNeuronParameter>
    {
        static abstract IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output);

        static abstract IEnumerable<string> GetInterneuronTags(
            VariableInfo variableInfo,
            InterneuronTagInfo? interneuronTagInfo = null
        );

        static abstract T Create(
            FunctionalParameter<BinaryNeuronParameter> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        );
    }
}
