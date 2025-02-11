using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    internal static class ProcessHelper
    {
        public static IGranny ParseBuild<TGranny, TGrannyWriter, TParameterSet, TAggregate>(
            TGrannyWriter grannyWriter,
            Network network,
            TParameterSet parameters,
            Action<TGranny, TAggregate> aggregateUpdater,
            TAggregate aggregate
        )
            where TGranny : IGranny
            where TGrannyWriter : IGrannyProcessor<TGranny, TParameterSet>, IGrannyWriter<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IDeductiveParameterSet
            where TAggregate : IGranny
        {
            TGranny result = grannyWriter.ParseBuild<TGranny, TGrannyWriter, TParameterSet>(
                network,
                parameters
            );

            aggregateUpdater(result, aggregate);

            return result;
        }
    }
}
