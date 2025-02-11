using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    internal static class ProcessHelper
    {
        public static IGranny TryParse<TGranny, TGrannyReader, TParameterSet, TAggregate>(
            TGrannyReader grannyReader,
            Network network,
            TParameterSet readParameters,
            Action<TGranny, TAggregate> aggregateUpdater,
            TAggregate aggregate
        )
            where TGranny : IGranny
            where TGrannyReader : IGrannyProcessor<TGranny, TParameterSet>, IGrannyReader<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IInductiveParameterSet
        {
            IGranny result = null;

            if (grannyReader.TryParse(
                network,
                readParameters,
                out TGranny gr)
                )
            {
                aggregateUpdater(gr, aggregate);
                result = gr;
            }

            return result;
        }

        internal static IGreatGrannyInfoSuperset<TResult> CreateGreatGrannyCandidateSets<TParameterSet, TResult>(
            Network network,
            Neuron granny,
            IEnumerable<TParameterSet> parameters,
            Func<Neuron, TParameterSet, IGreatGrannyInfo<TResult>> selector,
            IGreatGrannyProcess<TResult> target
        )
            where TResult : IGranny
            where TParameterSet : IParameterSet
        => GreatGrannyInfoSuperset<TResult>.Create(
            parameters.Select(
                p => new GreatGrannyInfoSet<TResult>(
                    network.GetPostsynapticNeurons(granny.Id).Select(gc => selector(gc, p)),
                    target
                )
            )
        );

        internal static GreatGrannyInfoSet<TResult> CreateGreatGrannyCandidateSet<TResult>(
            Network network,
            Neuron granny,
            Func<Neuron, IGreatGrannyInfo<TResult>> selector,
            IGreatGrannyProcess<TResult> target
        )
            where TResult : IGranny
        => new GreatGrannyInfoSet<TResult>(
            network.GetPostsynapticNeurons(granny.Id).Select(gc => selector(gc)),
            target
        );
    }
}
