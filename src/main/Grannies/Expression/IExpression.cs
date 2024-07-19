using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IExpression : IGranny
    {
        IList<IUnit> Units { get; }
    }
}