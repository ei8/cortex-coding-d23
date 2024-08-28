using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public interface IInstanceParameterSet : IClassReadParameterSet
    {
        IEnumerable<IPropertyAssociationParameterSet> PropertyAssociationsParameters { get; }
    }
}
