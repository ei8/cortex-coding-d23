using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IValue : IGranny<IValue, IValueParameterSet>
    {
        IInstantiatesClass InstantiatesClass { get; set; }
    }
}
