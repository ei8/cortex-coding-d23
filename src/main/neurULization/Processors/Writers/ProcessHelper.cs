using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    internal static class ProcessHelper
    {
        public static IGranny ParseBuild<TGranny, TGrannyProcessor, TParameterSet, TAggregate>(
            TGrannyProcessor grannyProcessor,
            Ensemble ensemble,
            TParameterSet parameters,
            Action<TGranny, TAggregate> aggregateUpdater,
            TAggregate aggregate
        )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>, IGrannyWriteProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IDeductiveParameterSet
            where TAggregate : IGranny
        {
            TGranny result = grannyProcessor.ParseBuild<TGranny, TGrannyProcessor, TParameterSet>(
                ensemble,
                parameters
            );

            aggregateUpdater(result, aggregate);

            return result;
        }
    }
}
