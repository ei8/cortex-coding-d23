namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyValueExpression : IPropertyValueExpression
    {
        public IValueExpression GreatGranny { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }

        public IGranny GetGreatGranny() => this.GreatGranny;
    }
}
