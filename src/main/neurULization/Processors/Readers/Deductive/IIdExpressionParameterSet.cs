using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IIdExpressionParameterSet : 
        IExpressionParameterSet
    {
        Guid Id { get; }
    }
}
