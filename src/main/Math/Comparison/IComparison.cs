using ei8.Cortex.Coding.d23.Math.TruthTables;

namespace ei8.Cortex.Coding.d23.Math.Comparison
{
    public interface IComparison
    {
    }

    public interface IComparison
    <
        TParam,
        TInterneuron
    > :
        IComparison,
        ITruthTable
        <
            TParam,
            TInterneuron
        >
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
    }
}
