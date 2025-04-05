using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors
{
    internal static class ProcessorExtensions
    {
        // TODO:1 simplify to eliminate code redundancy in calling functions
        internal static bool TryCreateGreatGranniesCore<TGranny, TParameterSet, TGreatGrannies>(
            this IGrannyProcessor<TGranny, TParameterSet> grannyProcessor,
            TryCreateGreatGranniesDelegate<TGreatGrannies> greatGranniesCreator,
            out TGreatGrannies result
        )
            where TGranny : IGranny
            where TParameterSet : IParameterSet
        {
            result = default;

            var tempResult = greatGranniesCreator(out bool bResult);

            if (bResult)
                result = tempResult;

            return bResult;
        }
    }
}
