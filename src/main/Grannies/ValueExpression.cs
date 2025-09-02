namespace ei8.Cortex.Coding.d23.Grannies
{
    /// <summary>
    /// An Expression with a single Head Unit - a "merge" of Unit Neuron and a Value.
    /// </summary>
    public class ValueExpression : IValueExpression
    {
        public IValue GreatGranny { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }

        public IGranny GetGreatGranny() => this.GreatGranny;
    }
}
