using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    internal static class ProcessHelper
    {
        public static bool TryParse<TGranny, TGrannyReader, TParameterSet, TAggregate>(
            TGrannyReader grannyReader,
            Network network,
            TParameterSet readParameters,
            Action<TGranny, TAggregate> aggregateUpdater,
            TAggregate aggregate,
            out TGranny result
        )
            where TGranny : IGranny
            where TGrannyReader : IGrannyProcessor<TGranny, TParameterSet>, IGrannyReader<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IInductiveParameterSet
        {
            result = default;
            var bResult = false;

            if (grannyReader.TryParse(
                network,
                readParameters,
                out TGranny gr)
                )
            {
                aggregateUpdater(gr, aggregate);
                result = gr;
                bResult = true;
            }

            return bResult;
        }

        internal static IGreatGrannyInfoSuperset<TResult> CreateGreatGrannyInfoSuperset<TParameterSet, TResult>(
            Network network,
            Neuron granny,
            IEnumerable<TParameterSet> parameters,
            Func<Neuron, TParameterSet, IGreatGrannyInfo<TResult>> selector,
            IGreatGrannyProcess<TResult> target
        )
            where TResult : IGranny
            where TParameterSet : IParameterSet
        {
            // TODO:1 Refactor duplicate code with overload (see below)
            var posts = network.GetPostsynapticNeurons(granny.Id);

            var ggis = parameters.Select(p =>
                new GreatGrannyInfoSet<TResult>(
                    posts.Select(post => selector(post, p)),
                    target
                )
            );

            return GreatGrannyInfoSuperset<TResult>.Create(ggis);
        }

        internal static IGreatGrannyInfoSuperset<TResult> CreateGreatGrannyInfoSuperset<TResult>(
            Network network,
            Neuron granny,
            Func<Neuron, IGreatGrannyInfo<TResult>> selector,
            IGreatGrannyProcess<TResult> target
        )
            where TResult : IGranny
        {
            // TODO:1 Refactor duplicate code with overload (see above)
            var posts = network.GetPostsynapticNeurons(granny.Id);

            var ggis = new GreatGrannyInfoSet<TResult>(
                posts.Select(post => selector(post)),
                target
            );

            return GreatGrannyInfoSuperset<TResult>.Create(ggis);
        }

        internal static IGreatGrannyInfoSuperset<TResult> CreatePermutatedGreatGrannyInfoSuperset<TResult>(
            Network network, 
            Neuron granny,
            Func<IEnumerable<Neuron>, IGreatGrannyInfo<TResult>> selector,
            IGreatGrannyProcess<TResult> target 
        )
            where TResult : IGranny
        {
            // TODO:1 Refactor duplicate code with overload (see above)
            IEnumerable<IEnumerable<Neuron>> posts = new[] { network.GetPostsynapticNeurons(granny.Id) };

            if (posts.First().Any())
                posts = ProcessHelper.Permutate(posts.First());

            var ggis = new GreatGrannyInfoSet<TResult>(
                posts.Select(ps => selector(ps)),
                target
            );

            return GreatGrannyInfoSuperset<TResult>.Create(ggis);
        }

        private static IEnumerable<IEnumerable<T>> Permutate<T>(IEnumerable<T> set, IEnumerable<T> subset = null)
        {
            if (subset == null) subset = new T[] { };
            if (!set.Any()) yield return subset;

            for (var i = 0; i < set.Count(); i++)
            {
                var newSubset = set.Take(i).Concat(set.Skip(i + 1));
                foreach (var permutation in Permutate(newSubset, subset.Concat(set.Skip(i).Take(1))))
                {
                    yield return permutation;
                }
            }
        }
    }
}
