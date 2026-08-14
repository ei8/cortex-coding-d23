using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public class InterneuronTagInfo
    {
        public static InterneuronTagInfo CreateByCommonTagPrefix(string value, int inputCount = 1) =>
            new(value, value, [..Enumerable.Repeat(value, inputCount - 1)]);
        
        public InterneuronTagInfo(string typeTagPrefix, string input1TagPrefix, params string[] otherInputTagPrefixes)
        {
            this.TypeTagPrefix = typeTagPrefix;
            this.InputTagPrefixes =  [input1TagPrefix, ..otherInputTagPrefixes];
        }

        public IEnumerable<string> InputTagPrefixes { get; }

        public string TypeTagPrefix { get; }
    }
}
