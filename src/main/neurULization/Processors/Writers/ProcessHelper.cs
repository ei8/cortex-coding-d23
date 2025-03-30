using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    internal static class ProcessHelper
    {
        public static bool TryParseBuild<TGranny, TGrannyWriter, TParameterSet, TAggregate>(
            TGrannyWriter grannyWriter,
            Network network,
            TParameterSet parameters,
            Action<TGranny, TAggregate> aggregateUpdater,
            TAggregate aggregate,
            out TGranny result
        )
            where TGranny : IGranny
            where TGrannyWriter : IGrannyProcessor<TGranny, TParameterSet>, IGrannyWriter<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IDeductiveParameterSet
            where TAggregate : IGranny
        {
            result = default;
            var bResult = false;

            try
            {
                TGranny tempResult = default;
                if (grannyWriter.TryParseBuild(
                    network,
                    parameters,
                    out tempResult
                ))
                {
                    result = tempResult;
                    aggregateUpdater(result, aggregate);
                    bResult = true;
                }
            }
            catch (Exception ex)
            {
                GrannyExtensions.Log($"Error: {ex}");
            }

            return bResult;
        }
    }
}
