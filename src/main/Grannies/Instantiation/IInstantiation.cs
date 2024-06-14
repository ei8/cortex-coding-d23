using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstantiation : IGranny<IInstantiation, IInstantiationParameterSet>
    {
        IInstantiatesClass InstantiatesClass { get; }
    }
}
