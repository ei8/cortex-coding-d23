namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstanceValueExpression : IInstanceValueExpression
    {
        public IInstanceValue GreatGranny { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }
    }
}
