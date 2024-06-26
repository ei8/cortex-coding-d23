﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface ISubordinationParameterSet : IAggregateParameterSet
    {
        IHeadParameterSet HeadParameters { get; }
        
        IEnumerable<IDependentParameterSet> DependentsParameters { get; }
    }
}
