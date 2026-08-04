using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class InterneuronTagInfo(IEnumerable<string> inputTagPrefixes, string typeTagPrefix)
    {
        public static InterneuronTagInfo CreateSameTagForSingleInput(string inputTagPrefix) =>
             new([inputTagPrefix], inputTagPrefix);

        public static InterneuronTagInfo CreateSameTagForDualInput(string inputTagPrefix) =>
             new([inputTagPrefix, inputTagPrefix], inputTagPrefix);

        public IEnumerable<string> InputTagPrefixes { get; } = inputTagPrefixes;

        public string TypeTagPrefix { get; } = typeTagPrefix;
    }
}
