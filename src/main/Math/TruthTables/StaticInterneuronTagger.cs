namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public class StaticInterneuronTagger(InterneuronTagInfo? value) : IStaticInterneuronTagger
    {
        public InterneuronTagInfo? Value { get; } = value;
    }
}
