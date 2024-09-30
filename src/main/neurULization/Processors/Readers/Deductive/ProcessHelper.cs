using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    internal static class ProcessHelper
    {
        public static IGranny TryParse<TGranny, TGrannyReader, TParameterSet, TAggregate>(
            TGrannyReader grannyReader,
            Ensemble ensemble,
            TParameterSet parameters,
            Action<TGranny, TAggregate> aggregateUpdater,
            TAggregate aggregate
        )
            where TGranny : IGranny
            where TGrannyReader : IGrannyProcessor<TGranny, TParameterSet>, IGrannyReader<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IDeductiveParameterSet
        {
            IGranny result = null;

            if (grannyReader.TryParse(
                ensemble,
                parameters,
                out TGranny gr)
                )
            {
                aggregateUpdater(gr, aggregate);
                result = gr;
            }

            return result;
        }
    }
}
