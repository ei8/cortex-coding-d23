using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class InterneuronTagInfo
    (
        string typeTagPrefix, 
        string input1TagPrefix, 
        params string[] otherInputTagPrefixes
    )
    {
        public static InterneuronTagInfo CreateByCommonTagPrefix(string value, int inputCount = 1) =>
            new(value, value, [..Enumerable.Repeat(value, inputCount - 1)]);

        public IEnumerable<string> InputTagPrefixes { get; } = [input1TagPrefix, .. otherInputTagPrefixes];

        public string TypeTagPrefix { get; } = typeTagPrefix;
    }
}
