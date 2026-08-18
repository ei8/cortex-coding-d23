using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NorGate
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
            NorGate, 
            FunctionalCircuitParameter
            <
                DualInputLogicGateBase.Input, 
                DualInputLogicGateBase.Output
            >,
            DualInputLogicGateBase.InterneuronSet
        >
    {
        public static NorGate Create
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
            output.Neuron0
        ];
    }
}
