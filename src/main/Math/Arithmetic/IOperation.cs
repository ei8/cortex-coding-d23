using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public interface IOperation<T> : ICircuit<FunctionalParameter<BinaryNeuronParameter>, BinaryNeuronParameter>
        where T : ICircuit<FunctionalParameter<BinaryNeuronParameter>, BinaryNeuronParameter>
    {
        static abstract FunctionalParameter<BinaryNeuronParameter> GetDefaultParameters(
            BinaryNeuronParameter? precedingValue,
            int exponent
        );

        static abstract IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworks(
            FunctionalParameter<BinaryNeuronParameter> parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        );

        static abstract T Create(
            FunctionalParameter<BinaryNeuronParameter> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        );
    }
}
