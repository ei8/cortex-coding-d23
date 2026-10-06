using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class AndGate
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
            AndGate,
            TwoInputOneOutputTruthTableBase.ParameterInfo,
            TwoInputTruthTableBase.Input,
            TwoInputOneOutputTruthTableBase.Output,
            TwoInputTruthTableBase.InterneuronSet
        >
    {
        public static AndGate Create
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
                    [outputs.Output1.Neuron0], 
                    [outputs.Output1.Neuron1]
                ] :
                [];
    }
}
