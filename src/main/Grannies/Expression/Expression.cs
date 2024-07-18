using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Expression : IExpression
    {
        public IList<IUnit> Units { get; } = new List<IUnit>();

        public Neuron Neuron { get; set; }
    }
}
