using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IPropertyAssignment : IGranny<IPropertyAssignment, IPropertyAssignmentParameterSet>
    {
        IPropertyValueExpression PropertyValueExpression { get; set; }

        IExpression Expression { get; set; }
    }
}
