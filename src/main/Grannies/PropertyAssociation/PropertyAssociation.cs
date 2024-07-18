namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyAssociation : IPropertyAssociation
    {
        public IPropertyAssignment PropertyAssignment { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }
    }
}
