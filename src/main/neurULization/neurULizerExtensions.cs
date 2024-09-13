using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Writers;
using Microsoft.Extensions.DependencyInjection;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public static class neurULizerExtensions
    {
        public static async Task<Ensemble> neurULizeAsync<TValue>(this neurULizer neurULizer, TValue value, Id23neurULizerWriteOptions options)
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(options, nameof(options));

            var result = new Ensemble();

            string valueClassKey = ExternalReference.ToKeyString(value.GetType());

            var propertyData = value.GetType().GetProperties()
                .Select(pi => pi.ToPropertyData(value))
                .Where(pd => pd != null);

            var neuronProperties = propertyData.Where(pd => pd.NeuronProperty != null).Select(pd => pd.NeuronProperty);
            var grannyProperties = propertyData.Where(pd => pd.NeuronProperty == null);
            Guid? regionId = neuronProperties.OfType<RegionIdProperty>().SingleOrDefault()?.Value;

            var propertyKeys = grannyProperties.Select(gp => gp.Key)
                .Concat(grannyProperties.Select(gp => gp.ClassKey))
                .Distinct();

            var ensembleRepository = options.ServiceProvider.GetRequiredService<IEnsembleRepository>();

            // use key to retrieve external reference url from library
            var externalReferences = await ensembleRepository.GetExternalReferencesAsync(
                options.UserId,
                new string[] {
                    valueClassKey
                }.Concat(
                    propertyKeys
                ).ToArray()
            );

            IdProperty idp = null;

            // Unnecessary to validate null id and tag values since another service can be
            // responsible for pruning grannies containing null or empty values.
            // Null values can also be considered as valid new values.
            var instance = await options.ServiceProvider.GetRequiredService<IInstanceProcessor>()
                .ObtainAsync<IInstance, IInstanceProcessor, Processors.Readers.Deductive.IInstanceParameterSet>(
                    result,
                    options,
                    new Processors.Readers.Deductive.InstanceParameterSet(
                        (idp = neuronProperties.OfType<IdProperty>().SingleOrDefault()) != null ?
                            idp.Value :
                            Guid.NewGuid(),
                        neuronProperties.OfType<TagProperty>().SingleOrDefault()?.Value,
                        neuronProperties.OfType<ExternalReferenceUrlProperty>().SingleOrDefault()?.Value,
                        regionId,
                        externalReferences[valueClassKey],
                        grannyProperties.Select(async gp =>
                            await neurULizerExtensions.CreatePropertyAssociationParams(options, gp, regionId, ensembleRepository, externalReferences)
                        )
                            .Select(t => t.Result)
                            .Where(i => i != null)
                            .ToArray()
                    )
                );

            var currentNeuronIds = result.GetItems<Neuron>()
                .Where(
                    n =>
                        n.IsTransient &&
                        result.GetPostsynapticNeurons(n.Id).All(postn => !postn.IsTransient)
                )
                .Select(n => n.Id);
            var nextNeuronIds = new List<Guid>();
            var processedNeuronIds = new List<Guid>();
            var removedNeuronIds = new List<Guid>();

            while (currentNeuronIds.Any())
            {
                nextNeuronIds.Clear();
                foreach (var currentNeuronId in currentNeuronIds.ToArray())
                {
                    Debug.WriteLine($"Optimizing '{currentNeuronId}'...");
                    if (removedNeuronIds.Contains(currentNeuronId))
                    {
                        Debug.WriteLine($"> Neuron replaced - skipped.");
                        continue;
                    }

                    AssertionConcern.AssertStateTrue(
                        result.TryGetById(currentNeuronId, out Neuron currentNeuron),
                        $"'currentNeuron' '{currentNeuronId}' must exist in ensemble."
                    );
                        
                    Debug.WriteLine($"Tag: '{currentNeuron.Tag}'");

                    var postsynaptics = result.GetPostsynapticNeurons(currentNeuronId);
                    if (postsynaptics.ContainsTransientUnprocessed(processedNeuronIds))
                    {
                        Debug.WriteLine($"> Transient unprocessed postsynaptic found - processing deferred.");
                        nextNeuronIds.Add(currentNeuronId);
                        continue;
                    }
                    else if (processedNeuronIds.Contains(currentNeuronId))
                    {
                        Debug.WriteLine($"> Already processed - skipped.");
                        continue;
                    }

                    var nextPostsynapticId = Guid.Empty;

                    if (currentNeuron.IsTransient)
                    {
                        Debug.WriteLine($"> Neuron marked as transient. Retrieving persistent identical granny with postsynaptics " +
                            $"'{string.Join(", ", postsynaptics.Select(n => n.Id))}'.");
                        var identical = await options.ServiceProvider.GetRequiredService<IEnsembleRepository>()
                            .GetPersistentIdentical(
                            postsynaptics,
                            options.UserId
                        );

                        if (identical.Item2 != null && currentNeuron.Tag == identical.Item2.Tag)
                        {
                            Debug.WriteLine($"> Persistent identical granny found - updating presynaptics and containing ensemble.");
                            neurULizerExtensions.UpdateDendrites(
                                result,
                                currentNeuronId,
                                identical.Item2.Id
                            );
                            neurULizerExtensions.RemoveTerminals(
                                result,
                                currentNeuronId
                            );
                            result.AddReplaceItems(identical.Item1);
                            result.Remove(currentNeuronId);
                            removedNeuronIds.Add(currentNeuronId);
                            Debug.WriteLine($"> Neuron replaced and removed.");
                            nextPostsynapticId = identical.Item2.Id;
                        }
                        else
                        {
                            Debug.WriteLine($"> Persistent identical granny was NOT found.");
                            nextPostsynapticId = currentNeuronId;
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"> Neuron NOT marked as transient.");
                        nextPostsynapticId = currentNeuronId;
                    }

                    processedNeuronIds.Add(nextPostsynapticId);
                    var presynaptics = result.GetPresynapticNeurons(nextPostsynapticId);
                    presynaptics.ToList().ForEach(n =>
                    {
                        Debug.WriteLine($"> Adding presynaptic '{n.Id}' to nextNeuronIds.");
                        nextNeuronIds.Add(n.Id);
                    });
                }
                Debug.WriteLine($"Setting next batch of {nextNeuronIds.Count()} ids.");
                currentNeuronIds = nextNeuronIds.ToArray();
            }

            return result;
        }

        private static async Task<Processors.Readers.Deductive.PropertyAssociationParameterSet> CreatePropertyAssociationParams(Id23neurULizerWriteOptions options, PropertyData gp, Guid? regionId, IEnsembleRepository ensembleRepository, IDictionary<string, Neuron> externalReferences)
        {
            return new Processors.Readers.Deductive.PropertyAssociationParameterSet(
                externalReferences[gp.Key],
                (
                    gp.ValueMatchBy == ValueMatchBy.Id ?
                        ((await ensembleRepository.GetByQueryAsync(
                            options.UserId,
                            new Library.Common.NeuronQuery()
                            {
                                Id = new string[] { gp.Value }
                            }
                        )).GetItems<Neuron>().Single()) :
                        Neuron.CreateTransient(gp.Value, null, regionId)
                ),
                externalReferences[gp.ClassKey],
                gp.ValueMatchBy
            );
        }

        private static void UpdateDendrites(Ensemble result, Guid oldPostsynapticId, Guid newPostsynapticId)
        {
            var currentDendrites = result.GetDendrites(oldPostsynapticId).ToArray();
            foreach (var currentDendrite in currentDendrites)
            {
                result.AddReplace(
                    new Terminal(
                        currentDendrite.Id,
                        currentDendrite.IsTransient,
                        currentDendrite.PresynapticNeuronId,
                        newPostsynapticId,
                        currentDendrite.Effect,
                        currentDendrite.Strength
                    )
                );
            }
        }

        private static void RemoveTerminals(Ensemble result, Guid presynapticId)
        {
            var terminals = result.GetTerminals(presynapticId).ToArray();
            foreach (var terminal in terminals)
                result.Remove(terminal.Id);
        }

        private static bool ContainsTransientUnprocessed(
            this IEnumerable<Neuron> posts,
            IEnumerable<Guid> processedNeuronIds
        ) => posts.Any(n => n.IsTransient && !processedNeuronIds.Contains(n.Id));

        private static async Task<Tuple<Ensemble, Neuron>> GetPersistentIdentical(
            this IEnsembleRepository ensembleRepository,
            IEnumerable<Neuron> postsynaptics,
            string userId
        )
        {
            IEnumerable<string> postsIds = postsynaptics.Select(n => n.Id.ToString());

            var similarGrannyFromDb = (await ensembleRepository.GetByQueryAsync(
                    userId,
                    new Library.Common.NeuronQuery()
                    {
                        Postsynaptic = postsIds,
                        DirectionValues = Library.Common.DirectionValues.Outbound,
                        Depth = 1
                    }
                ));
            var similarGrannyFromDbNeuron =
                similarGrannyFromDb.GetItems<Neuron>()
                .Where(n => !postsynaptics.Any(pn => pn.Id == n.Id));

            AssertionConcern.AssertStateTrue(
                similarGrannyFromDbNeuron.Count() < 2,
                    $"Redundant Neuron with postsynaptic Neurons '{string.Join(", ", postsIds)}' encountered."
                );
            if (similarGrannyFromDbNeuron.Any())
            {
                var resultTerminalCount = similarGrannyFromDb.GetTerminals(similarGrannyFromDbNeuron.Single().Id).Count();
                AssertionConcern.AssertStateTrue(
                    resultTerminalCount == postsynaptics.Count(),
                    $"A correct identical match should have '{postsynaptics.Count()} terminals. Result has {resultTerminalCount}'."
                );
            }

            return new Tuple<Ensemble, Neuron>(
                similarGrannyFromDb,
                similarGrannyFromDbNeuron.SingleOrDefault()
                );
        }

        public static async Task<IEnumerable<TValue>> DeneurULizeAsync<TValue>(this neurULizer neurULizer, Ensemble value, Id23neurULizerReadOptions options)
            where TValue : class, new()
        {
            List<TValue> result = new List<TValue>();

            string valueClassKey = ExternalReference.ToKeyString(typeof(TValue));

            // get properties
            var propertyData = typeof(TValue).GetProperties()
                .Select(pi => pi.ToPropertyData(new TValue()))
                .Where(pd => pd != null);

            var neuronProperties = propertyData.Where(pd => pd.NeuronProperty != null).Select(pd => pd.NeuronProperty);
            var grannyProperties = propertyData.Where(pd => pd.NeuronProperty == null);

            var propertyKeys = grannyProperties.Select(gp => gp.Key)
                .Concat(grannyProperties.Select(gp => gp.ClassKey))
                .Distinct();

            // use key to retrieve external reference url from library
            IEnsembleRepository ensembleRepository = options.ServiceProvider.GetRequiredService<IEnsembleRepository>();
            var externalReferences = await ensembleRepository
                .GetExternalReferencesAsync(
                    options.UserId,
                    (new string[] {
                        valueClassKey
                    }).Concat(
                        propertyKeys
                    ).ToArray()
                );

            var instanceNeurons = value.GetPresynapticNeurons(options.InstantiatesClass.Neuron.Id);

            foreach (var instanceNeuron in instanceNeurons)
            {
                if (options.ServiceProvider.GetRequiredService<Processors.Readers.Inductive.IInstanceProcessor>().TryParse(
                    value,
                    options,
                    new Processors.Readers.Inductive.InstanceParameterSet(
                        instanceNeuron,
                        externalReferences[valueClassKey],
                        grannyProperties.Select(gp =>
                            Processors.Readers.Inductive.PropertyAssociationParameterSet.CreateWithoutGranny(
                                externalReferences[gp.Key],
                                externalReferences[gp.ClassKey]
                            )
                        )
                    ),
                    out IInstance instance
                ))
                {
                    var tempResult = new TValue();

                    foreach (var gp in grannyProperties)
                    {
                        var propAssoc = instance.PropertyAssociations.SingleOrDefault(
                            pa => pa.PropertyAssignment.Expression.Units
                                .AsEnumerable()
                                .GetValueUnitGranniesByTypeId(options.Primitives.Unit.Id).SingleOrDefault().Value.Id == externalReferences[gp.Key].Id
                        );
                        object propValue = null;

                        var property = tempResult.GetType().GetProperty(gp.PropertyName);
                        var classAttribute = property.GetCustomAttributes<neurULClassAttribute>().SingleOrDefault();

                        if (classAttribute != null)
                        {
                            AssertionConcern.AssertArgumentValid(
                                t => property.PropertyType == typeof(Guid),
                                typeof(TValue),
                                $"Property '{property.Name}' has '{nameof(neurULClassAttribute)}' but its Type is not equal to 'Guid'.",
                                nameof(TValue)
                                );

                            propValue = propAssoc.PropertyAssignment.PropertyValueExpression.ValueExpression.Value.Neuron.Id;
                        }
                        else
                        {
                            AssertionConcern.Equals(gp.ClassKey, ExternalReference.ToKeyString(property.PropertyType));

                            var propValueString = propAssoc.PropertyAssignment.PropertyValueExpression.ValueExpression.Value.Neuron.Tag;
                            if (property.PropertyType == typeof(string))
                            {
                                propValue = propValueString;
                            }
                            else if (property.PropertyType == typeof(Guid))
                            {
                                propValue = Guid.Parse(propValueString);
                            }
                            else if (
                                Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTimeOffset) ||
                                property.PropertyType == typeof(DateTimeOffset)
                                )
                            {
                                propValue = DateTimeOffset.Parse(propValueString);
                            }
                            // TODO: else use neurULConverterAttribute
                        }
                        property.SetValue(tempResult, propValue);
                    }

                    foreach (var np in neuronProperties)
                    {

                    }

                    result.Add(tempResult);
                }

                if (options.OperationOptions.Mode == ReadMode.First && result.Count == 1)
                    break;
            }

            return result.AsEnumerable();
        }        
    }
}
