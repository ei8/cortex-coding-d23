using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Comparison
{
    public class GreaterThan
    (
        In2Out1TruthTableBase.ParameterInfo parameters,
        In2TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        In2Out1LogicGateBase<GreaterThan>
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ILogicGateStatic
        <
            GreaterThan,
            In2Out1TruthTableBase.ParameterInfo,
            In2TruthTableBase.Input,
            In2Out1TruthTableBase.Output,
            In2TruthTableBase.InterneuronSet
        >
    {
        public static GreaterThan Create
        (
            In2Out1TruthTableBase.ParameterInfo parameters,
            In2TruthTableBase.InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) =>
            new
            (
                parameters,
                interneurons,
                variableInfo
            );

        public static IEnumerable<IEnumerable<Neuron>> GetOutputsPerInterneuron(In2Out1TruthTableBase.Output outputs) =>
            outputs.Output1 != null ?
                [
                    [outputs.Output1.Neuron0],
                    [outputs.Output1.Neuron0],
                    [outputs.Output1.Neuron1],
                    [outputs.Output1.Neuron0]
                ] :
                [];
    }
}
