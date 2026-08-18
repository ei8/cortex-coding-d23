using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class XnorGate
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
            XnorGate, 
            FunctionalCircuitParameter
            <
                DualInputLogicGateBase.Input, 
                DualInputLogicGateBase.Output
            >,
            DualInputLogicGateBase.InterneuronSet
        >
    {
        public static XnorGate Create
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
            output.Neuron1,
            output.Neuron0,
            output.Neuron0,
            output.Neuron1
        ];
    }
}
