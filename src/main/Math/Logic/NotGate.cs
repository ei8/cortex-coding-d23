using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class NotGate
    (
        NotGate.ParameterInfo parameters,
        NotGate.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        OneInputTruthTableBase
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ILogicGate
        <
            OneInputTruthTableBase.ParameterInfo,
            OneInputTruthTableBase.InterneuronSet
        >,
        ITruthTableStatic
        <
            NotGate,
            OneInputTruthTableBase.ParameterInfo,
            OneInputTruthTableBase.Input,
            OneInputTruthTableBase.Output,
            OneInputTruthTableBase.InterneuronSet
        >
    {
        public static NotGate Create(
            OneInputTruthTableBase.ParameterInfo parameters,
            OneInputTruthTableBase.InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => new(
            parameters,
            interneurons,
            variableInfo
        );

        public static IEnumerable<IEnumerable<Neuron>> GetOutputsPerInterneuron(OneInputTruthTableBase.Output outputs) =>
            outputs.Output1 != null ?
                [
                    [outputs.Output1.Neuron1],
                    [outputs.Output1.Neuron0]
                ] :
                [];
    }
}
