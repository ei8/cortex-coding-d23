using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class AndGate : DualInputLogicGateBase, IDualInputLogicGate<AndGate>
    {
        protected AndGate(
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

        public static AndGate Create(
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
            output.Neuron0, 
            output.Neuron0, 
            output.Neuron0, 
            output.Neuron1
        ];
    }
}
