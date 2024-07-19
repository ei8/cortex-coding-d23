using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstanceParameterSet : IParameterSet
    {
        Neuron Class { get; }

        IEnumerable<IPropertyAssociationParameterSet> PropertyAssociationsParameters { get; }
    }
}
