using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Comparison
{
    public  class LessThan
    (
        TwoInputTruthTableBase.ParameterInfo parameters,
        TwoInputTruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TwoInputLogicGateBase
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ITruthTableStatic
        <
            LessThan,
            TwoInputTruthTableBase.ParameterInfo,
            TwoInputTruthTableBase.InterneuronSet
        >
    {
        public static LessThan Create
        (
            TwoInputTruthTableBase.ParameterInfo parameters,
            TwoInputTruthTableBase.InterneuronSet interneurons,
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
            output.Neuron0,
            output.Neuron0
        ];
    }
}
