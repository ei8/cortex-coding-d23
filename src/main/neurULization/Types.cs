using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public delegate bool GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TAggregate>(
        TGrannyProcessor grannyProcessor,
        Network network,
        TParameterSet parameters,
        Action<TGranny, TAggregate> aggregateUpdater,
        TAggregate aggregate,
        out TGranny result
    )
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
        where TAggregate : IGranny;
}
