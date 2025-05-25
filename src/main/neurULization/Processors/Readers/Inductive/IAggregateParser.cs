using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public interface IAggregateParser
    {
        bool TryParse<TResultDerived, TResult, TParameterSet>(
            Neuron granny,
            ILesserGrannyReader<TResult, TParameterSet> grannyReader,
            TParameterSet parameters,
            Network network,
            IMirrorSet mirrors,
            out TResult result
        )
            where TResultDerived : TResult, new()
            where TResult : IGranny
            where TParameterSet : IParameterSet;
    }
}
