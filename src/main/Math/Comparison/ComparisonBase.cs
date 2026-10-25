using ei8.Cortex.Coding.d23.Math.TruthTables;

namespace ei8.Cortex.Coding.d23.Math.Comparison
{
    public abstract class ComparisonBase
    (
        In2Out1TruthTableBase.ParameterInfo parameters,
        In2TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        In2TruthTableBase
        <
            In2Out1TruthTableBase.ParameterInfo,
            In2TruthTableBase.Input,
            In2TruthTableBase.InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        IComparison
        <
            In2Out1TruthTableBase.ParameterInfo,
            In2TruthTableBase.InterneuronSet
        >
    {
    }
}
