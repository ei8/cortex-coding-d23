using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NotGate
    (
        In1Out1TruthTableBase.ParameterInfo parameters,
        In1TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        In1Out1LogicGateBase<NotGate>
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ILogicGateStatic
        <
            NotGate,
            In1Out1TruthTableBase.ParameterInfo,
            In1TruthTableBase.Input,
            In1Out1TruthTableBase.Output,
            In1TruthTableBase.InterneuronSet
        >
    {
        public static NotGate Create(
            In1Out1TruthTableBase.ParameterInfo parameters,
            In1TruthTableBase.InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => new(
            parameters,
            interneurons,
            variableInfo
        );

        public static IEnumerable<IEnumerable<Neuron>> GetOutputsPerInterneuron(In1Out1TruthTableBase.Output outputs) =>
            outputs.Output1 != null ?
                [
                    [outputs.Output1.Neuron1],
                    [outputs.Output1.Neuron0]
                ] :
                [];
    }
}
