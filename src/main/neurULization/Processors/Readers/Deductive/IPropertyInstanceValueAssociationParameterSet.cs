namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IPropertyInstanceValueAssociationParameterSet : 
        IPropertyParameterSetCore, 
        IDeductiveParameterSet,  
        IPropertyAssociationParameterSet
    {
        Neuron Value { get; }

        Neuron Class { get; }

        ValueMatchBy ValueMatchBy { get; }
    }
}
