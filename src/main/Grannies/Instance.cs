using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Instance : IInstance
    {
        public IInstantiatesClass InstantiatesClass { get; set; }

        public IList<IPropertyValueAssociation> PropertyValueAssociations { get; set; } = new List<IPropertyValueAssociation>();

        public Neuron Neuron { get; set; }
    }
}
