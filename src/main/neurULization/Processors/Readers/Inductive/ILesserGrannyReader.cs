using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public interface ILesserGrannyReader<TGranny, TParameterSet> : IGrannyReader<TGranny, TParameterSet>
        where TGranny : IGranny
        where TParameterSet : IParameterSet
    {
        bool TryCreateGreatGrannies(
            TParameterSet parameters,
            Network network,
            IExternalReferenceSet externalReferences,
            out IGreatGrannyInfoSuperset<TGranny> result
        );
    }
}
