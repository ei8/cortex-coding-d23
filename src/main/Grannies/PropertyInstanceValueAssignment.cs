namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyInstanceValueAssignment : IPropertyInstanceValueAssignment
    {
        public IPropertyInstanceValueExpression TypedGreatGranny { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }

        public IGranny GreatGranny { 
            get => this.TypedGreatGranny; 
            set => this.TypedGreatGranny = (IPropertyInstanceValueExpression)value; 
        }
    }
}
