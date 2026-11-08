namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public interface IInterneuronTagger
    {
    }

    public interface IStaticInterneuronTagger : IInterneuronTagger
    {
        InterneuronTagInfo? Value { get; }
    }

    public interface IVariableInterneuronTagger : IInterneuronTagger
    {
        InterneuronTagInfo? CreateByVariable(VariableInfo variableInfo);
    }

    public interface IKeyedInterneuronTagger : IInterneuronTagger
    {
        InterneuronTagInfo? CreateByKey(string key, VariableInfo variableInfo);
    }
}
