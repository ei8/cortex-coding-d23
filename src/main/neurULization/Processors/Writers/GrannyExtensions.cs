using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Library.Common;
using Microsoft.Extensions.DependencyInjection;
using Nancy.TinyIoc;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public static class GrannyExtensions
    {
        internal async static Task<TGranny> ObtainAsync<TGranny, TGrannyWriteProcessor, TWriteParameterSet>(
            this TGrannyWriteProcessor grannyWriteProcessor,
            Ensemble ensemble,
            Id23neurULizerWriteOptions options,
            TWriteParameterSet writeParameters
            )
            where TGranny : IGranny
            where TGrannyWriteProcessor : IGrannyWriteProcessor<TGranny, TWriteParameterSet>
            where TWriteParameterSet : IDeductiveParameterSet
        => await grannyWriteProcessor.ObtainAsync<TGranny, TGrannyWriteProcessor, TWriteParameterSet>(
            new ProcessParameters(
                ensemble,
                options
                ),
            writeParameters
            );

        /// <summary>
        /// Retrieves granny from ensemble if present, otherwise, builds and adds it to the ensemble.
        /// </summary>
        /// <typeparam name="TGranny"></typeparam>
        /// <typeparam name="TParameterSet"></typeparam>
        /// <param name="grannyWriteProcessor"></param>
        /// <param name="processParameters"></param>
        /// <param name="writeParameters"></param>
        /// <returns></returns>
        internal async static Task<TGranny> ObtainAsync<TGranny, TGrannyWriteProcessor, TParameterSet>(
            this TGrannyWriteProcessor grannyWriteProcessor,
            ProcessParameters processParameters,
            TParameterSet writeParameters
            )
            where TGranny : IGranny
            where TGrannyWriteProcessor : IGrannyWriteProcessor<TGranny, TParameterSet>
            where TParameterSet : IDeductiveParameterSet
        {
            AssertionConcern.AssertArgumentNotNull(processParameters, nameof(processParameters));
            AssertionConcern.AssertArgumentNotNull(writeParameters, nameof(writeParameters));

            TGranny result = default;
            // if target is not in specified ensemble
            // TODO:DEL remove ReadProcessor?
            if (!grannyWriteProcessor.ReadProcessor.TryParse(processParameters.Ensemble, (Id23neurULizerWriteOptions)processParameters.Options, writeParameters, out result))
                // build in ensemble
                result = await grannyWriteProcessor.BuildAsync(processParameters.Ensemble, (Id23neurULizerWriteOptions)processParameters.Options, writeParameters);

            return result;
        }

        internal static async Task<TResult> AggregateBuildAsync<TResult>(
            this TResult tempResult,
            IEnumerable<IGreatGrannyInfo<TResult>> processes,
            IEnumerable<IGreatGrannyProcessAsync<TResult>> targets,
            Ensemble ensemble,
            Id23neurULizerWriteOptions options,
            Func<Neuron> grannyNeuronCreator = null,
            Func<TResult, IEnumerable<Neuron>> postsynapticsRetriever = null
        )
            where TResult : IGranny
        {
            IGranny precedingGranny = null;

            var ts = targets.ToArray();
            for (int i = 0; i < ts.Length; i++)
            {
                precedingGranny = await ts[i].ExecuteAsync(
                    processes.ElementAt(i),
                    ensemble,
                    options,
                    precedingGranny,
                    tempResult
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
