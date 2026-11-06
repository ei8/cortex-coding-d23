using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NotGate
    (
        In1Out1TruthTable.ParameterInfo parameters,
        In1TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        In1TruthTableBase
        <
            NotGate,
            In1Out1TruthTable.ParameterInfo,
            In1TruthTableBase.Input,
            In1Out1TruthTable.Output
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ITruthTableStatic
        <
            NotGate,
            In1Out1TruthTable.ParameterInfo,
            In1TruthTableBase.Input,
            In1Out1TruthTable.Output,
            In1TruthTableBase.InterneuronSet
        >
    {
        public static NotGate Create(
            In1Out1TruthTable.ParameterInfo parameters,
            In1TruthTableBase.InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => 
            new
            (
                parameters,
                interneurons,
                variableInfo
            );

        public static IEnumerable<IEnumerable<Neuron>> GetOutputsPerInterneuron(In1Out1TruthTable.Output outputs) =>
            outputs.Output1 != null ?
                [
                    [outputs.Output1.Neuron1],
                    [outputs.Output1.Neuron0]
                ] :
                [];
    }
}
