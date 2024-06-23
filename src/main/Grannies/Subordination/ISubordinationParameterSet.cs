﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface ISubordinationParameterSet : IAggregateParameterSet
    {
        IUnitParameterSet HeadParameters { get; }
        
        IEnumerable<IUnitParameterSet> DependentsParameters { get; }
    }
}
