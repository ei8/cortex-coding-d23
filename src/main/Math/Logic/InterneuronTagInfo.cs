using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class InterneuronTagInfo(IEnumerable<string> inputTagPrefixes, string typeTagPrefix)
    {
        public IEnumerable<string> InputTagPrefixes { get; } = inputTagPrefixes;

        public string TypeTagPrefix { get; } = typeTagPrefix;
    }
}
