using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using ei8.Cortex.Coding.d23.neurULization.Processors.Writers;
using ei8.Cortex.Coding.Properties;
using ei8.Cortex.Coding.Properties.Neuron;
using ei8.Cortex.Coding.Reflection;
using ei8.Cortex.Coding.Wrappers;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public Network neurULize<TValue>(
            TValue value, 
            neurULizerTypeInfo typeInfo,
            IDictionary<Guid, Coding.Neuron> idPropertyValueNeurons,
            IDictionary<string, Coding.Neuron> externalReferences
        )
            where TValue : class
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(typeInfo, nameof(typeInfo));
            AssertionConcern.AssertArgumentNotNull(idPropertyValueNeurons, nameof(idPropertyValueNeurons));
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));

            var network = new Network();
            Guid? regionId = typeInfo.NeuronProperties.OfType<RegionIdProperty>().SingleOrDefault()?.Value;
            IdProperty idp = null;

            if (!(value is IInstanceValueWrapper ivw))
            {
                // Unnecessary to validate null id and tag values since another service can be
                // responsible for pruning grannies containing null or empty values.
                // Null values can also be considered as valid new values.
                this.options.InstanceWriter.TryParseBuild<
                    IInstance,
                    IInstanceWriter,
                    Processors.Readers.Deductive.IInstanceParameterSet
                >(
                    network,
                    new Processors.Readers.Deductive.InstanceParameterSet(
                        (idp = typeInfo.NeuronProperties.OfType<IdProperty>().SingleOrDefault()) != null ?
                            idp.Value :
                            Guid.NewGuid(),
                        typeInfo.NeuronProperties.OfType<TagProperty>().SingleOrDefault()?.Value,
                        typeInfo.NeuronProperties.OfType<ExternalReferenceUrlProperty>().SingleOrDefault()?.Value,
                        regionId,
                        externalReferences[typeInfo.ValueClassKey],
                        typeInfo.GrannyProperties.Select(gp =>
                            {
                                IPropertyAssociationParameterSet paps = null;

                                var valueNeuron = gp.ValueMatchBy == ValueMatchBy.Id ?
                                    idPropertyValueNeurons[Guid.Parse(gp.Value)] :
                                    Neuron.CreateTransient(gp.Value, null, regionId);

                                if (!string.IsNullOrWhiteSpace(gp.ClassKey))
                                {
                                    paps = new PropertyInstanceValueAssociationParameterSet(
                                        externalReferences[gp.Key],
                                        valueNeuron,
                                        externalReferences[gp.ClassKey],
                                        gp.ValueMatchBy
                                    );
                                }
                                else
                                {
                                    paps = new PropertyValueAssociationParameterSet(
                                        externalReferences[gp.Key],
                                        valueNeuron
                                    );
                                }

                                return paps;
                            }
                            )
                            .Where(i => i != null)
                    ),
                    out IInstance instance
                );
            }
            else
            {
                this.options.IdInstanceValueWriter.TryParseBuild<
                    IInstanceValue,
                    IIdInstanceValueWriter,
                    Processors.Readers.Deductive.IIdInstanceValueParameterSet
                >(
                    network,
                    new Processors.Readers.Deductive.IdInstanceValueParameterSet(
                        Neuron.CreateTransient(ivw.Tag, string.Empty, null),
                        externalReferences[typeInfo.ValueClassKey],
                        ValueMatchBy.Tag,
                        ivw.Id
                    ),
                    out IInstanceValue instance
                );
            }

            return network;
        }

        public IEnumerable<TValue> DeneurULize<TValue>(
            Network value,
            IEnumerable<Neuron> instanceNeurons,
            neurULizerTypeInfo typeInfo,
            IDictionary<string, Coding.Neuron> externalReferences
        )
            where TValue : class, new()
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(instanceNeurons, nameof(instanceNeurons));
            AssertionConcern.AssertArgumentNotNull(typeInfo, nameof(typeInfo));
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));

            List<TValue> result = new List<TValue>();

            foreach (var instanceNeuron in instanceNeurons)
            {
                if (this.options.InductiveInstanceReader.TryParse(
                    value,
                    new Processors.Readers.Inductive.InstanceParameterSet(
                        instanceNeuron,
                        externalReferences[typeInfo.ValueClassKey],
                        typeInfo.GrannyProperties.Select(gp =>
                            {
                                IPropertyAssociationParameterSet paps = null;
                                if (!string.IsNullOrWhiteSpace(gp.ClassKey))
                                {
                                    paps = Processors.Readers.Inductive.PropertyInstanceValueAssociationParameterSet.CreateWithoutGranny(
                                        externalReferences[gp.Key],
                                        externalReferences[gp.ClassKey]
                                    );
                                }
                                else
                                {
                                    paps = Processors.Readers.Inductive.PropertyValueAssociationParameterSet.CreateWithoutGranny(
                                        externalReferences[gp.Key]
                                    );
                                }
                                return paps;
                            }
                        )
                    ),
                    out IInstance instance
                ))
                {
                    var tempResult = new TValue();

                    foreach (var gp in typeInfo.GrannyProperties)
                    {
                        var propAssoc = instance.PropertyAssociations.SingleOrDefault(
                            pa => pa.HasPropertyAssignment(
                                this.options.ExternalReferences.Unit, 
                                // property neuron
                                externalReferences[gp.Key]
                            )
                        );
                        object propValue = null;

                        var property = tempResult.GetType().GetProperty(gp.PropertyName);
                        var classAttribute = property.GetCustomAttributes<neurULClassAttribute>().SingleOrDefault();

                        AssertionConcern.AssertStateTrue(
                            propAssoc.TryGetPropertyValue(out IGranny propValueGranny),
                            "Property Association does not contain expected Value."
                        );

                        if (classAttribute != null)
                        {
                            AssertionConcern.AssertArgumentValid(
                                t => property.PropertyType == typeof(Guid),
                                typeof(TValue),
                                $"Property '{property.Name}' has '{nameof(neurULClassAttribute)}' but its Type is not equal to 'Guid'.",
                                nameof(TValue)
                                );
                        
                            propValue = propValueGranny.Neuron.Id;
                        }
                        else
                        {
                            AssertionConcern.Equals(gp.ClassKey, ExternalReference.ToKeyString(property.PropertyType));

                            string propValueString = propValueGranny.GetValueTag(this.options.ExternalReferences.NominalSubject.Id);

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
