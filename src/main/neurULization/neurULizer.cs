﻿using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Writers;
using ei8.Cortex.Coding.Properties;
using ei8.Cortex.Coding.Properties.Neuron;
using ei8.Cortex.Coding.Reflection;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class neurULizer : IneurULizer
    {
        private readonly Id23neurULizerOptions options;

        public neurULizer(Id23neurULizerOptions options)
        {
            AssertionConcern.AssertArgumentNotNull(options, nameof(options));

            this.options = options;
        }

        public async Task<Ensemble> neurULizeAsync<TValue>(
            TValue value, 
            neurULizerTypeInfo typeInfo,
            IDictionary<string, Coding.Neuron> idPropertyValueNeurons,
            IDictionary<string, Coding.Neuron> externalReferences
        )
            where TValue : class
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(typeInfo, nameof(typeInfo));
            AssertionConcern.AssertArgumentNotNull(idPropertyValueNeurons, nameof(idPropertyValueNeurons));
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));

            var result = new Ensemble();
            Guid? regionId = typeInfo.NeuronProperties.OfType<RegionIdProperty>().SingleOrDefault()?.Value;
            IdProperty idp = null;

            // Unnecessary to validate null id and tag values since another service can be
            // responsible for pruning grannies containing null or empty values.
            // Null values can also be considered as valid new values.
            var instance = await this.options.WritersInstanceProcessor.ObtainAsync<
                IInstance, 
                IInstanceProcessor, 
                Processors.Readers.Deductive.IInstanceParameterSet
            >(
                result,
                new Processors.Readers.Deductive.InstanceParameterSet(
                    (idp = typeInfo.NeuronProperties.OfType<IdProperty>().SingleOrDefault()) != null ?
                        idp.Value :
                        Guid.NewGuid(),
                    typeInfo.NeuronProperties.OfType<TagProperty>().SingleOrDefault()?.Value,
                    typeInfo.NeuronProperties.OfType<ExternalReferenceUrlProperty>().SingleOrDefault()?.Value,
                    regionId,
                    externalReferences[typeInfo.ValueClassKey],
                    typeInfo.GrannyProperties.Select(gp =>
                            new Processors.Readers.Deductive.PropertyAssociationParameterSet(
                                externalReferences[gp.Key],
                                gp.ValueMatchBy == ValueMatchBy.Id ?
                                    idPropertyValueNeurons[gp.Value] :
                                    Neuron.CreateTransient(gp.Value, null, regionId),
                                externalReferences[gp.ClassKey],
                                gp.ValueMatchBy
                            )
                        )
                        .Where(i => i != null)
                )
            );

            return result;
        }

        public async Task<IEnumerable<TValue>> DeneurULizeAsync<TValue>(
            Ensemble value,
            IEnumerable<Neuron> instanceNeurons,
            neurULizerTypeInfo typeInfo,
            IDictionary<string, Coding.Neuron> externalReferences
        )
            where TValue : class, new()
        {
            List<TValue> result = new List<TValue>();

            foreach (var instanceNeuron in instanceNeurons)
            {
                if (this.options.ReadersInductiveInstanceProcessor.TryParse(
                    value,
                    new Processors.Readers.Inductive.InstanceParameterSet(
                        instanceNeuron,
                        externalReferences[typeInfo.ValueClassKey],
                        typeInfo.GrannyProperties.Select(gp =>
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

                    foreach (var gp in typeInfo.GrannyProperties)
                    {
                        var propAssoc = instance.PropertyAssociations.SingleOrDefault(
                            pa => pa.PropertyAssignment.Expression.Units
                                .AsEnumerable()
                                .GetValueUnitGranniesByTypeId(this.options.Primitives.Unit.Id).SingleOrDefault().Value.Id == externalReferences[gp.Key].Id
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

                    foreach (var np in typeInfo.NeuronProperties)
                    {
                        var instanceNeuronProperty = instance.Neuron.GetType().GetProperty(
                            np.GetType().Name.Replace("Property", string.Empty)
                        );
                        object propertyValue = instanceNeuronProperty.GetValue(instance.Neuron);

                        tempResult.GetType().GetProperty(np.Name).SetValue(tempResult, propertyValue);
                    }

                    result.Add(tempResult);
                }
            }

            return result.AsEnumerable();
        }

        public IneurULizerOptions Options => this.options;
    }
}
