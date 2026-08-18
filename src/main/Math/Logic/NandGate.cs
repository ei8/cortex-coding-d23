using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NandGate
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
            NandGate,
            FunctionalCircuitParameter
            <
                DualInputLogicGateBase.Input,
                DualInputLogicGateBase.Output
            >,
            DualInputLogicGateBase.InterneuronSet
        >
    {
        public static NandGate Create
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
            output.Neuron1,
            output.Neuron1,
            output.Neuron0
        ];
    }
}
