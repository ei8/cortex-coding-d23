using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IPropertyAssociation : IGranny<IPropertyAssociation, IPropertyAssociationParameterSet>
    {
        IPropertyAssignment PropertyAssignment { get; set; }

        IExpression Expression { get; set; }
    }
}
