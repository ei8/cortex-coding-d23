using ei8.Cortex.Coding.d23.Math.TruthTables;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class TwoInputLogicGateBase
    (
        TwoInputTruthTableBase.ParameterInfo parameters,
        TwoInputTruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TwoInputTruthTableBase
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ILogicGate
        <
            TwoInputTruthTableBase.ParameterInfo,
            TwoInputTruthTableBase.InterneuronSet
        >
    {
    }
}
