using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public interface ILesserGrannyWriter<TGranny, TParameterSet> : IGrannyWriter<TGranny, TParameterSet>
        where TGranny : IGranny
        where TParameterSet : IDeductiveParameterSet
    {
        bool TryCreateGreatGrannies(
            TParameterSet parameters,
            Network network,
            IMirrorSet mirrors,
            out IEnumerable<IGreatGrannyInfo<TGranny>> result
        );
    }
}
