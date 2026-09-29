using ei8.Cortex.Coding.d23.Math.TruthTables;

namespace ei8.Cortex.Coding.d23.Math.Comparison
{
    public abstract class ComparisonBase
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
        IComparison
        <
            TwoInputTruthTableBase.ParameterInfo,
            TwoInputTruthTableBase.InterneuronSet
        >
    {
    }
}
