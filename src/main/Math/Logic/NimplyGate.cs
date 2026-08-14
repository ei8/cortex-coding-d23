using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NimplyGate : DualInputLogicGateBase, ILogicGate<NimplyGate, DualInputLogicGateBase.Input, DualInputLogicGateBase.Output>
    {
        protected NimplyGate(
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

        public static NimplyGate Create(
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
            output.Neuron0,
            output.Neuron1,
            output.Neuron0
        ];
    }
}
