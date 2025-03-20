namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstanceValue : IInstanceValue
    {
        public IUnit Value { get; set; }

        public IInstantiatesClass TypedGreatGranny { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }
    }
}
