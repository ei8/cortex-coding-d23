using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class OrGate
    (
        FunctionalCircuitParameter<DualInputLogicGateBase.Input, DualInputLogicGateBase.Output> parameters,
        DualInputLogicGateBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) : 
        DualInputLogicGateBase
        (
            parameters,
            interneurons,
            variableInfo
        ), 
        ILogicGate
        <
            OrGate, 
            FunctionalCircuitParameter<
                DualInputLogicGateBase.Input, 
                DualInputLogicGateBase.Output
            >,
            DualInputLogicGateBase.InterneuronSet
        >
    {
        public static OrGate Create
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
            output.Neuron1
        ];
    }
}
