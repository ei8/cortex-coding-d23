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

            // use key to retrieve external reference url from library
            var externalReferences = await options.EnsembleRepository.GetExternalReferencesAsync(
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
            var instance = await options.InstanceProcessor.ObtainAsync<IInstance, IInstanceProcessor, Processors.Readers.Deductive.IInstanceParameterSet>(
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
                            await neurULizerExtensions.CreatePropertyAssociationParams(options, gp, regionId, options.EnsembleRepository, externalReferences)
                        )
                            .Select(t => t.Result)
                            .Where(i => i != null)
                            .ToArray()
                    )
                );

            await options.EnsembleRepository.Uniquify(
                options.UserId,
                result,
                options.Cache
            );

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
            var externalReferences = await options.EnsembleRepository
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
                if (options.InstanceProcessor.TryParse(
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
