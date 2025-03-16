namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyValueExpression : IPropertyValueExpression
    {
        public IValueExpression TypedGreatGranny { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }

        public IGranny GreatGranny
        {
            get => this.TypedGreatGranny;
            set => this.TypedGreatGranny = (IValueExpression) value;
        }
    }
}
