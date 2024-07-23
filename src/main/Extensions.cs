using ei8.Cortex.Coding.d23.Grannies;
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
using System.Reflection;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    public static class Extensions
    {
        #region IGranny
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
            for(int i = 0; i < ts.Length; i++)
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
        #endregion

        #region Units
        internal static IEnumerable<IUnit> GetByTypeId(this IEnumerable<IUnit> units, Guid id, bool isEqual = true) =>
            units.Where(u => isEqual ? u.Type.Id == id : u.Type.Id != id);

        internal static IEnumerable<IUnitParameterSet> GetByTypeId(this IEnumerable<IUnitParameterSet> units, Guid id, bool isEqual = true) =>
            units.Where(u => isEqual ? u.Type.Id == id : u.Type.Id != id);
        #endregion

        public static bool HasSameElementsAs<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second
            )
        {
            var firstMap = first
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            var secondMap = second
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            return
                firstMap.Keys.All(x =>
                    secondMap.Keys.Contains(x) && firstMap[x] == secondMap[x]
                ) &&
                secondMap.Keys.All(x =>
                    firstMap.Keys.Contains(x) && secondMap[x] == firstMap[x]
                );
        }

        internal static PropertyData ToPropertyData(this PropertyInfo property, object obj)
        {
            PropertyData result = null;
            var ignore = property.GetCustomAttributes<neurULIgnoreAttribute>().SingleOrDefault();
            if (ignore == null)
            {
                var neuronPropertyAttribute = property.GetCustomAttributes<neurULNeuronPropertyAttribute>().SingleOrDefault();
                if (neuronPropertyAttribute != null)
                {
                    result = Extensions.GetNeuronPropertyData(neuronPropertyAttribute, property.Name, property.GetValue(obj));
                }
                else
                {
                    var classAttribute = property.GetCustomAttributes<neurULClassAttribute>().SingleOrDefault();
                    // if property type is Guid and property is decorated by classAttribute
                    var matchBy = property.PropertyType == typeof(Guid) && classAttribute != null ?
                            // match by id
                            ValueMatchBy.Id :
                            // otherwise, match by tag
                            ValueMatchBy.Tag;
                    var propertyValue = property.GetValue(obj)?.ToString();
                    string propertyKey = property.ToExternalReferenceKeyString();

                    result = new PropertyData(
                        propertyKey,
                        // if classAttribute was specified
                        classAttribute?.Type != null ? 
                            // use classAttribute type
                            classAttribute.Type.ToExternalReferenceKeyString() :
                            // otherwise, use property type
                            property.PropertyType.ToExternalReferenceKeyString(),
                        propertyValue,
                        matchBy
                        );
                }
            }

            return result;
        } 

        private static PropertyData GetNeuronPropertyData(neurULNeuronPropertyAttribute neuronPropertyAttribute, string propertyName, object propertyValue)
        {
            PropertyData result = null;

            if (!neuronPropertyAttribute.IsReadOnly)
            {
                var property = neuronPropertyAttribute.PropertyName ?? propertyName;
                INeuronProperty neuronProperty;
                switch (property)
                {
                    case nameof(Neuron.Id):
                        neuronProperty = new IdProperty((Guid)propertyValue);
                        break;
                    case nameof(Neuron.Tag):
                        neuronProperty = new TagProperty((string)
                            propertyValue);
                        break;
                    case nameof(Neuron.ExternalReferenceUrl):
                        neuronProperty = new ExternalReferenceUrlProperty((string)propertyValue);
                        break;
                    case nameof(Neuron.RegionId):
                        neuronProperty = new RegionIdProperty((Guid?)propertyValue);
                        break;
                    default:
                        throw new NotImplementedException($"Neuron Property '{property}' not yet implemented.");
                }

                if (neuronProperty != null)
                    result = new PropertyData(neuronProperty);
            }

            return result;
        }

        internal static string GetExternalReferenceKey(this MemberInfo value)
        {
            // get ExternalReferenceKeyAttribute of root type
            var erka = value.GetCustomAttributes<neurULKeyAttribute>().SingleOrDefault();
            string key;
            // if attribute exists
            if (erka != null)
                key = erka.Key;
            else if (value is PropertyInfo pi)
                key = pi.ToExternalReferenceKeyString();
            else if (value is Type t)
                // assembly qualified name 
                key = t.ToExternalReferenceKeyString();
            else
                throw new ArgumentOutOfRangeException(nameof(value));
            return key;
        }
    }
}
