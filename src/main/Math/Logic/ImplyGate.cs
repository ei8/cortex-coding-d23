using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class ImplyGate : DualInputLogicGateBase, IDualInputLogicGate<ImplyGate>
    {
        protected ImplyGate(
            FunctionalParameter<BinaryNeuronParameter> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) : base(
            parameters,
            networks,
            variableInfo
        )
        {
        }

        public static ImplyGate Create(
            FunctionalParameter<BinaryNeuronParameter> parameters,
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
            output.Neuron0,
            output.Neuron1
        ];
    }
}
