using ei8.Cortex.Coding.d23.Math.TruthTables;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class In2Out1LogicGateBase
    (
        In2Out1TruthTableBase.ParameterInfo parameters,
        In2TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        In2Out1TruthTableBase
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ILogicGate
        <
            In2Out1TruthTableBase.ParameterInfo,
            In2TruthTableBase.InterneuronSet
        >
    {
    }
}
