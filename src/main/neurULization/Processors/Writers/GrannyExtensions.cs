using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public static class GrannyExtensions
    {
        /// <summary>
        /// Retrieves granny from ensemble if present, otherwise, builds and adds it to the ensemble.
        /// </summary>
        /// <typeparam name="TGranny"></typeparam>
        /// <typeparam name="TParameterSet"></typeparam>
        /// <param name="grannyWriter"></param>
        /// <param name="ensemble"></param>
        /// <param name="options"></param>
        /// <param name="writeParameters"></param>
        /// <returns></returns>
        public static TGranny ParseBuild<TGranny, TGrannyWriter, TParameterSet>(
            this TGrannyWriter grannyWriter,
            Ensemble ensemble,
            TParameterSet writeParameters
            )
            where TGranny : IGranny
            where TGrannyWriter : IGrannyWriter<TGranny, TParameterSet>
            where TParameterSet : IDeductiveParameterSet
        {
            AssertionConcern.AssertArgumentNotNull(ensemble, nameof(ensemble));
            AssertionConcern.AssertArgumentNotNull(writeParameters, nameof(writeParameters));

            TGranny result = default;
            // if target is not in specified ensemble
            if (!grannyWriter.Reader.TryParse(
                ensemble,
                writeParameters, 
                out result
                ))
                // build in ensemble
                result = grannyWriter.Build(
                    ensemble,
                    writeParameters
                );

            return result;
        }

        internal static TResult AggregateBuild<TResult>(
            this TResult tempResult,
            IEnumerable<IGreatGrannyInfo<TResult>> processes,
            IEnumerable<IGreatGrannyProcess<TResult>> targets,
            Ensemble ensemble,
            Func<Neuron> grannyNeuronCreator = null,
            Func<TResult, IEnumerable<Neuron>> postsynapticsRetriever = null
        )
            where TResult : IGranny
        {
            IGranny precedingGranny = null;

            var ts = targets.ToArray();
            for (int i = 0; i < ts.Length; i++)
            {
                var candidate = processes.ElementAt(i);

                if(ts[i].TryGetParameters(
                    precedingGranny,
                    candidate,
                    out IParameterSet parameters
                ))
                    precedingGranny = ts[i].Execute(
                        candidate,
                        ensemble,
                        tempResult,
                        parameters
                    );
            }

            tempResult.Neuron = grannyNeuronCreator != null ?
                grannyNeuronCreator() :
                precedingGranny.Neuron;

            IEnumerable<Neuron> postsynaptics = null;
            if (
                postsynapticsRetriever != null &&
                (postsynaptics = postsynapticsRetriever(tempResult)) != null &&
                postsynaptics.Any()
                )
                postsynaptics.ToList().ForEach(n =>
                    ensemble.AddReplace(
                        Terminal.CreateTransient(
                            tempResult.Neuron.Id, n.Id
                        )
                    )
                );

            return tempResult;
        }
    }
}
