using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class OrGate : DualInputLogicGateBase, ILogicGate<OrGate, DualInputLogicGateBase.Input, DualInputLogicGateBase.Output>
    {
        protected OrGate(
            FunctionalCircuitParameter<DualInputLogicGateBase.Input, DualInputLogicGateBase.Output> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) : base(
            parameters,
            networks,
            variableInfo
        )
        {
        }

        public static OrGate Create(
            FunctionalCircuitParameter<DualInputLogicGateBase.Input, DualInputLogicGateBase.Output> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) => new(
            parameters,
            networks,
            variableInfo
        );

        public static IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output) =>
        [
            output.Neuron0,
            output.Neuron1,
            output.Neuron1,
            output.Neuron1
        ];
    }
}
