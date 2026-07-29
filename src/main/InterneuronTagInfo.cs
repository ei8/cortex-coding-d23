namespace ei8.Cortex.Coding.d23
{
    public class InterneuronTagInfo(string[] inputTagPrefixes, string typeTagPrefix)
    {
        public static InterneuronTagInfo CreateSameTagForSingleInput(string inputTagPrefix) =>
             new([inputTagPrefix], inputTagPrefix);

        public static InterneuronTagInfo CreateSameTagForDualInput(string inputTagPrefix) =>
             new([inputTagPrefix, inputTagPrefix], inputTagPrefix);

        public string[] InputTagPrefixes { get; } = inputTagPrefixes;

        public string TypeTagPrefix { get; } = typeTagPrefix;
    }
}
