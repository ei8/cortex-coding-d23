namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IPropertyValueAssociationParameterSet : 
        IPropertyParameterSet, 
        IDeductiveParameterSet,
        IPropertyAssociationParameterSet
    {
        Neuron Value { get; }
    }
}
