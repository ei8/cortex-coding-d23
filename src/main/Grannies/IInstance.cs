using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstance : IGranny
    {
        IInstantiatesClass InstantiatesClass { get; set; }

        IList<IPropertyValueAssociation> PropertyValueAssociations { get; }
    }
}
