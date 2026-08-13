using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public interface ISequentialOperation<T> : ICircuit<FunctionalParameter<NeuronParameterBase>, NeuronParameterBase>
        where T : ICircuit<FunctionalParameter<NeuronParameterBase>, NeuronParameterBase>
    {
        static abstract FunctionalParameter<NeuronParameterBase> GetDefaultParameters(
            UnaryNeuronParameter? currentDigit,
            BinaryNeuronParameter? input1,
            BinaryNeuronParameter? input2,
            BinaryNeuronParameter? precedingValue,
            UnaryNeuronParameter? nextDigit,
            BinaryNeuronParameter? result,
            BinaryNeuronParameter? regrouping
        );

        static abstract IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworks(
            FunctionalParameter<NeuronParameterBase> parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        );

        static abstract T Create(
            FunctionalParameter<NeuronParameterBase> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        );
    }
}
