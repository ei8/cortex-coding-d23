using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class XorGate :
        DualInputLogicGateBase,
        ILogicGate
        <
            XorGate, 
            DualInputLogicGateBase.Input, 
            DualInputLogicGateBase.Output, 
            DualInputLogicGateBase.InterneuronSet
        >
    {
        protected XorGate
        (
            FunctionalCircuitParameter<DualInputLogicGateBase.Input, DualInputLogicGateBase.Output> parameters,
            DualInputLogicGateBase.InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) :
            base
            (
                parameters,
                interneurons,
                variableInfo
            )
        {
        }

        public static XorGate Create
        (
            FunctionalCircuitParameter<DualInputLogicGateBase.Input, DualInputLogicGateBase.Output> parameters,
            DualInputLogicGateBase.InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => 
            new
            (
                parameters,
                interneurons,
                variableInfo
            );

        public static IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output) =>
        [
            output.Neuron0,
            output.Neuron1,
            output.Neuron1,
            output.Neuron0
        ];
    }
}
