using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    internal static class ProcessHelper
    {
        public static IGranny TryParse<TGranny, TGrannyProcessor, TParameterSet, TResult>(
            TGrannyProcessor grannyReadProcessor,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TParameterSet readParameters,
            Action<TGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>, IGrannyReadProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IReadParameterSet
        {
            IGranny result = null;

            if (grannyReadProcessor.TryParse(
                ensemble,
                (Id23neurULizerReadOptions) options,
                readParameters,
                out TGranny gr)
                )
            {
                resultUpdater(gr, tempResult);
                result = gr;
            }

            return result;
        }

        internal static IEnumerable<IGreatGrannyInfo<TResult>> CreateGreatGrannyCandidates<TResult>(
            Ensemble ensemble,
            Neuron granny,
            Func<Neuron, IEnumerable<IGreatGrannyInfo<TResult>>> selector
        )
            where TResult : IGranny
            =>
            granny != null ?
                // use each postsynaptic of granny as a granny candidate
                ensemble.GetPostsynapticNeurons(granny.Id).SelectMany(
                    gc => selector(gc)
                ) :
                Array.Empty<IGreatGrannyInfo<TResult>>();

        internal static IEnumerable<IGreatGrannyInfo<TResult>> CreateGreatGrannyCandidates<TResult>(
            Ensemble ensemble,
            Neuron granny,
            Func<Neuron, IGreatGrannyInfo<TResult>> selector
        )
            where TResult : IGranny
            =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble, 
                granny,
                gc => new IGreatGrannyInfo<TResult>[] { selector(gc) }
                );
    }
}
