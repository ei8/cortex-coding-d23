namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IPropertyValueAssociationParameterSet : 
        IPropertyParameterSetCore, 
        IDeductiveParameterSet,
        IPropertyAssociationParameterSet
    {
        Neuron Value { get; }
    }
}
