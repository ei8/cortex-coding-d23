namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    internal interface IInductiveGreatGrannyInfo<TAggregate> : 
        IGreatGrannyInfo<TAggregate>
    {
        Neuron Neuron { get; }
    }
}
