namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyValueAssignment : IPropertyValueAssignment
    {
        public IPropertyValueExpression GreatGranny { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }

        public object GetGreatGranny() => this.GreatGranny;
    }
}
