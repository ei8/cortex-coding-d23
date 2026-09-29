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

        public static IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output) =>
        [
            output.Neuron1,
            output.Neuron0
        ];
    }
}
