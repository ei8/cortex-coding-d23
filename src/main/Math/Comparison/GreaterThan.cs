using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Comparison
{
    public class GreaterThan
    (
        TwoInputOneOutputTruthTableBase.ParameterInfo parameters,
        TwoInputTruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TwoInputOneOutputLogicGateBase
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ITruthTableStatic
        <
            GreaterThan,
            TwoInputOneOutputTruthTableBase.ParameterInfo,
            TwoInputTruthTableBase.Input,
            TwoInputOneOutputLogicGateBase.Output,
            TwoInputTruthTableBase.InterneuronSet
        >
    {
        public static GreaterThan Create
        (
            TwoInputOneOutputTruthTableBase.ParameterInfo parameters,
            TwoInputTruthTableBase.InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) =>
            new
            (
                parameters,
                interneurons,
                variableInfo
            );

        public static IEnumerable<IEnumerable<Neuron>> GetOutputsPerInterneuron(TwoInputOneOutputTruthTableBase.Output outputs) =>
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
