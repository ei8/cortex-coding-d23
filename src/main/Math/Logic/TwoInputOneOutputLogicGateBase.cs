using ei8.Cortex.Coding.d23.Math.TruthTables;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class TwoInputOneOutputLogicGateBase
    (
        TwoInputOneOutputTruthTableBase.ParameterInfo parameters,
        TwoInputTruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TwoInputOneOutputTruthTableBase
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ILogicGate
        <
            TwoInputOneOutputTruthTableBase.ParameterInfo,
            TwoInputTruthTableBase.InterneuronSet
        >
    {
    }
}
