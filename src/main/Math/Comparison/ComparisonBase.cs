using ei8.Cortex.Coding.d23.Math.TruthTables;

namespace ei8.Cortex.Coding.d23.Math.Comparison
{
    public abstract class ComparisonBase
    (
        TwoInputOneOutputTruthTableBase.ParameterInfo parameters,
        TwoInputTruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TwoInputTruthTableBase
        <
            TwoInputOneOutputTruthTableBase.ParameterInfo,
            TwoInputTruthTableBase.Input,
            TwoInputTruthTableBase.InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        IComparison
        <
            TwoInputOneOutputTruthTableBase.ParameterInfo,
            TwoInputTruthTableBase.InterneuronSet
        >
    {
    }
}
