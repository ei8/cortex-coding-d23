using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public interface ISequentialOperation<T, TInput, TOutput> : ICircuit<FunctionalCircuitParameter<TInput, TOutput>>
        where T : ICircuit<FunctionalCircuitParameter<TInput, TOutput>>
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
        // TODO: fix?
        static abstract FunctionalCircuitParameter<TInput, TOutput> GetDefaultParameters(
            UnaryNeuronParameter? currentDigit,
            BinaryNeuronParameter? input1,
            BinaryNeuronParameter? input2,
            BinaryNeuronParameter? precedingValue,
            UnaryNeuronParameter? nextDigit,
            BinaryNeuronParameter? result,
            BinaryNeuronParameter? regrouping
        );

        static abstract IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworks(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        );

        static abstract T Create(
            FunctionalCircuitParameter<TInput, TOutput> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        );
    }
}
