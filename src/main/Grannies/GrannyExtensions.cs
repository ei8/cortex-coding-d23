using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using Microsoft.Extensions.DependencyInjection;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    internal static class GrannyExtensions
    {
        internal async static Task<TGranny> ObtainAsync<TGranny, TGrannyProcessor, TParameterSet>(
            this TGrannyProcessor grannyProcessor,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TParameterSet parameters
            )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet
        => await grannyProcessor.ObtainAsync<TGranny, TGrannyProcessor, TParameterSet>(
            new ObtainParameters(
                ensemble,
                options
                ),
            parameters
            );

        /// <summary>
        /// Retrieves granny from ensemble if present; Otherwise, retrieves it from persistence or builds it, and adds it to the ensemble.
        /// </summary>
        /// <typeparam name="TGranny"></typeparam>
        /// <typeparam name="TParameterSet"></typeparam>
        /// <param name="grannyProcessor"></param>
        /// <param name="ensemble"></param>
        /// <param name="parameters"></param>
        /// <param name="ensembleRepository"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal async static Task<TGranny> ObtainAsync<TGranny, TGrannyProcessor, TParameterSet>(
            this TGrannyProcessor grannyProcessor,
            ObtainParameters obtainParameters,
            TParameterSet parameters
            )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet
        {
            AssertionConcern.AssertArgumentNotNull(obtainParameters, nameof(obtainParameters));
            AssertionConcern.AssertArgumentNotNull(parameters, nameof(parameters));

            TGranny result = default;
            // if target is not in specified ensemble
            if (!grannyProcessor.TryParse(obtainParameters.Ensemble, obtainParameters.Options, parameters, out TGranny ensembleParseResult))
            {
                // retrieve target from DB
                var grannyQueries = grannyProcessor.GetQueries(obtainParameters.Options, parameters);

                await grannyQueries.Process(obtainParameters, new List<IGranny>());

                // if target is in DB
                if (grannyProcessor.TryParse(obtainParameters.Ensemble, obtainParameters.Options, parameters, out TGranny dbParseResult))
                {
                    result = dbParseResult;
                }
                // else if target is not in DB 
                else
                {
                    // build in ensemble
                    result = await grannyProcessor.BuildAsync(obtainParameters.Ensemble, obtainParameters.Options, parameters);
                }
            }
            // if target was found in ensemble
            else if (ensembleParseResult != null)
                result = ensembleParseResult;

            return result;
        }

        internal static async Task<bool> Process(
            this IEnumerable<IGrannyQuery> grannyQueries,
            ObtainParameters obtainParameters,
            IList<IGranny> retrievedGrannies,
            bool breakBeforeLastGetQuery = false
            )
        {
            var result = false;
            // loop through each grannyQuery
            foreach (var grannyQuery in grannyQueries)
            {
                // if last is supposed to be skipped
                if (breakBeforeLastGetQuery && grannyQueries.Last() == grannyQuery)
                {
                    // indicate success then break
                    result = true;
                    break;
                }

                NeuronQuery query = null;

                // if query is obtained successfully
                if ((query = await grannyQuery.GetQuery(obtainParameters, retrievedGrannies)) != null)
                {
                    // get ensemble based on parameters and previous granny neuron if it's assigned
                    var queryResult = await obtainParameters.Options.ServiceProvider.GetRequiredService<IEnsembleRepository>().GetByQueryAsync(
                        obtainParameters.Options.UserId,
                        query
                        );
                    // enrich ensemble
                    obtainParameters.Ensemble.AddReplaceItems(queryResult);
                    // if granny query is retriever
                    if (grannyQuery is IRetriever gqr)
                    {
                        var retrievalResult = gqr.RetrieveGranny(obtainParameters.Ensemble, obtainParameters.Options, retrievedGrannies.AsEnumerable());
                        if (retrievalResult != null)
                            retrievedGrannies.Add(retrievalResult);
                        // if retrieval fails and this is not the last query
                        else if (grannyQueries.Last() != grannyQuery)
                            // break with a failure indication
                            break;
                    }
                }
                else
                    break;

                // if this is the last granny
                if (grannyQueries.Last() == grannyQuery)
                {
                    // indicate success
                    result = true;
                }
            }
            return result;
        }

        internal static void TryParseCore<TGranny, TGrannyProcessor, TParameterSet>(
            this TGrannyProcessor grannyProcessor,
            TParameterSet parameters,
            Ensemble ensemble,
            TGranny tempResult,
            IEnumerable<Guid> selection,
            LevelParser[] levelParsers,
            System.Action<Neuron> grannySetter,
            ref TGranny result
            )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet
        {
            foreach (var levelParser in levelParsers)
                selection = levelParser.Evaluate(ensemble, selection);

            Trace.WriteLineIf(selection.Count() > 1, "Redundant data encountered.");
            if (selection.Count() == 1 && ensemble.TryGetById(selection.Single(), out Neuron ensembleResult))
            {
                grannySetter(ensembleResult);
                result = tempResult;
            }
        }

        internal static bool AggregateTryParse<TResult>(
            this TResult tempResult,
            IEnumerable<IGreatGrannyInfo<TResult>> processes,
            IEnumerable<IGreatGrannyProcess<TResult>> targets,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            out TResult result,
            bool setGrannyNeuronOnCompletion = true
        )
            where TResult : IGranny
        {
            result = default;

            IGranny precedingGranny = null;
            var ts = targets.ToArray();
            for (int i = 0; i < ts.Length; i++)
            {
                if ((precedingGranny = ts[i].Execute(
                    processes.ElementAt(i),
                    ensemble,
                    options,
                    precedingGranny,
                    tempResult
                    )) == null)
                    break;
                else if (i == ts.Length - 1)
                {
                    if (setGrannyNeuronOnCompletion)
                        tempResult.Neuron = precedingGranny.Neuron;
                    result = tempResult;
                }
            }

            return result != null;
        }

        internal static async Task<TResult> AggregateBuildAsync<TResult>(
            this TResult tempResult,
            IEnumerable<IGreatGrannyInfo<TResult>> processes,
            IEnumerable<IGreatGrannyProcessAsync<TResult>> targets,
            Ensemble ensemble,
            Id23neurULizerOptions options,
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

        // TODO: see if useful later
        // public static IServiceCollection AddGrannies(this IServiceCollection services)
        //{
        //    AssertionConcern.AssertArgumentNotNull(services, nameof(services));

        //    services.TryAdd(ServiceDescriptor.Transient<IExpressionProcessor, ExpressionProcessor>());
        //    services.TryAdd(ServiceDescriptor.Transient<IExpressionProcessor, ExpressionProcessor>());
        //    services.TryAdd(ServiceDescriptor.Transient<IInstantiatesClassProcessor, InstantiatesClassProcessor>());
        //    services.TryAdd(ServiceDescriptor.Transient<IPropertyAssignmentProcessor, PropertyAssignmentProcessor>());
        //    services.TryAdd(ServiceDescriptor.Transient<IPropertyAssociationProcessor, PropertyAssociationProcessor>());
        //    services.TryAdd(ServiceDescriptor.Transient<IPropertyValueExpressionProcessor, PropertyValueExpressionProcessor>());
        //    services.TryAdd(ServiceDescriptor.Transient<IUnitProcessor, UnitProcessor>());
        //    services.TryAdd(ServiceDescriptor.Transient<IValueProcessor, ValueProcessor>());
        //    services.TryAdd(ServiceDescriptor.Transient<IValueExpressionProcessor, ValueExpressionProcessor>());
        //    services.TryAdd(ServiceDescriptor.Transient<IInstanceProcessor, InstanceProcessor>());

        //    return services;
        //}
    }
}
