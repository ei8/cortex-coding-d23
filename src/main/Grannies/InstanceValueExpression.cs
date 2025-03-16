namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstanceValueExpression : IInstanceValueExpression
    {
        public IInstanceValue TypedGreatGranny { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }
        public IGranny GreatGranny
        {
            get => this.TypedGreatGranny;
            set => this.TypedGreatGranny = (IInstanceValue) value;
        }
    }
}
