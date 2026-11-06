using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NandGate
    (
        In2Out1TruthTable.ParameterInfo parameters,
        In2TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        In2TruthTableBase
        <
            NandGate,
            In2Out1TruthTable.ParameterInfo,
            In2TruthTableBase.Input,
            In2Out1TruthTable.Output
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ITruthTableStatic
        <
            NandGate,
            In2Out1TruthTable.ParameterInfo,
            In2TruthTableBase.Input,
            In2Out1TruthTable.Output,
            In2TruthTableBase.InterneuronSet
        >
    {
        public static NandGate Create
        (
            In2Out1TruthTable.ParameterInfo parameters,
            In2TruthTableBase.InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => 
            new
            (
                parameters,
                interneurons,
                variableInfo
            );

        public static IEnumerable<IEnumerable<Neuron>> GetOutputsPerInterneuron(In2Out1TruthTable.Output outputs) =>
            outputs.Output1 != null ?
                [
                    [outputs.Output1.Neuron1],
                    [outputs.Output1.Neuron1],
                    [outputs.Output1.Neuron1],
                    [outputs.Output1.Neuron0]
                ] :
                [];
    }
}
