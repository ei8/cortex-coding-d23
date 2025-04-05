using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TGranny"></typeparam>
    /// <typeparam name="TParameterSet"></typeparam>
    public interface ILesserGrannyReader<TGranny, TParameterSet> : IGrannyReader<TGranny, TParameterSet>
        where TGranny : IGranny
        where TParameterSet : IDeductiveParameterSet
    {
        bool TryCreateGreatGrannies(
            TParameterSet parameters,
            Network network,
            IExternalReferenceSet externalReferences,
            out IEnumerable<IGreatGrannyInfo<TGranny>> result
        );
    }
}
