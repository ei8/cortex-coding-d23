using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    internal static class ProcessHelper
    {
        public static IGranny TryParse<TGranny, TGrannyProcessor, TParameterSet, TAggregate>(
            TGrannyProcessor grannyReadProcessor,
            Ensemble ensemble,
            TParameterSet readParameters,
            Action<TGranny, TAggregate> aggregateUpdater,
            TAggregate aggregate
        )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>, IGrannyReadProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IInductiveParameterSet
        {
            IGranny result = null;

            if (grannyReadProcessor.TryParse(
                ensemble,
                readParameters,
                out TGranny gr)
                )
            {
                aggregateUpdater(gr, aggregate);
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
        {
            var result = default(IEnumerable<IGreatGrannyInfo<TResult>>);
            if (granny != null)
            {
                // use each postsynaptic of granny as a granny candidate
                var posts = ensemble.GetPostsynapticNeurons(granny.Id);
                result = posts.SelectMany(
                    gc => selector(gc)
                );
            }
            else
                result = Array.Empty<IGreatGrannyInfo<TResult>>();

            return result;
        }

        internal static IEnumerable<IGreatGrannyInfo<TResult>> CreateGreatGrannyCandidates<TResult>(
            Ensemble ensemble,
            Neuron granny,
            Func<Neuron, IGreatGrannyInfo<TResult>> selector
        )
            where TResult : IGranny
            =>
            CreateGreatGrannyCandidates(
                ensemble,
                granny,
                gc => new IGreatGrannyInfo<TResult>[] { selector(gc) }
                );
    }
}
