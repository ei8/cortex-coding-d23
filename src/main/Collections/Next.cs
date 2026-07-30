using System.Linq;

namespace ei8.Cortex.Coding.d23.Collections
{
    public class Next : AdjacentBase
    {
        public Next() : base()
        {
        }

        protected override string GetInterneuronTag(VariableInfo variableInfo) =>
            $"{variableInfo.Function}({variableInfo.Inputs.First()})";
    }
}
