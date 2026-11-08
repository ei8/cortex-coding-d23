using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public class TwoInputInterneuronTagger : IVariableInterneuronTagger
    {
        public InterneuronTagInfo? CreateByVariable(VariableInfo variableInfo) =>
            InterneuronTagInfo.CreateByCommonTagPrefix
                (
                    variableInfo.Inputs.First(),
                    2
                );
    }
}
