namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyValueAssociation : IPropertyValueAssociation
    {
        public IPropertyValueAssignment GreatGranny { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }
    }
}
