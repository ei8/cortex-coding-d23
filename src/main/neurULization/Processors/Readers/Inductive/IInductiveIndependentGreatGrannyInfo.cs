using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public interface IInductiveIndependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate> : IIndependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate>, IInductiveGreatGrannyInfo<TAggregate>
        where TGranny : IGranny
        where TProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
    }
}