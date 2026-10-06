using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NorGate
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
            NorGate, 
            TwoInputOneOutputTruthTableBase.ParameterInfo,
            TwoInputTruthTableBase.Input,
            TwoInputOneOutputTruthTableBase.Output,
            TwoInputTruthTableBase.InterneuronSet
        >
    {
        public static NorGate Create
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
                    [outputs.Output1.Neuron1],
                    [outputs.Output1.Neuron0],
                    [outputs.Output1.Neuron0],
                    [outputs.Output1.Neuron0]
                ] :
                [];
    }
}
