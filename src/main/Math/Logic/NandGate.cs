using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NandGate : DualInputLogicGateBase, ILogicGate<NandGate, DualInputLogicGateBase.Input, DualInputLogicGateBase.Output>
    {
        protected NandGate(
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

        public static NandGate Create(
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
            output.Neuron1,
            output.Neuron1,
            output.Neuron1,
            output.Neuron0
        ];
    }
}
