namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstanceValue : IInstanceValue
    {
        public IInstantiatesClass GreatGranny { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }

        public object GetGreatGranny() => this.GreatGranny;
    }
}
