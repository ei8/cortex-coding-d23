using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Instance : IInstance
    {
        public IInstantiatesClass InstantiatesClass { get; set; }

        public IList<IPropertyAssociation> PropertyAssociations { get; set; }

        public Neuron Neuron { get; set; }
    }
}
