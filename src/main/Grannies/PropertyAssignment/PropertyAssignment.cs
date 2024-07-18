namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyAssignment : IPropertyAssignment
    {
        public IPropertyValueExpression PropertyValueExpression { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }
    }
}
