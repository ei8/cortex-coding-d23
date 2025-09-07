using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
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
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ei8.Cortex.Coding.d23.neurULization.Implementation
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
            IEnumerable<Coding.Neuron> idPropertyValueNeurons,
            neurULizerTypeInfo typeInfo,
            IDictionary<string, Coding.Neuron> mirrors
        )
            where TValue : class
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(typeInfo, nameof(typeInfo));
            AssertionConcern.AssertArgumentNotNull(idPropertyValueNeurons, nameof(idPropertyValueNeurons));
            AssertionConcern.AssertArgumentNotNull(mirrors, nameof(mirrors));

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
                        typeInfo.NeuronProperties.OfType<MirrorUrlProperty>().SingleOrDefault()?.Value,
                        regionId,
                        mirrors[typeInfo.ValueClassKey],
                        typeInfo.GrannyProperties.Select(gp =>
                            {
                                IPropertyAssociationParameterSet paps = null;

                                var valueNeuron = gp.ValueMatchBy == ValueMatchBy.Id ?
                                    idPropertyValueNeurons.Single(ipvn => ipvn.Id == Guid.Parse(gp.Value)) :
                                    Neuron.CreateTransient(gp.Value, null, regionId);

                                if (neurULizer.IsValueAssociation(gp))
                                {
                                    // TODO:1 validate that valueNeuron represents an Instance of type gp.ClassKey
                                    // if(!string.IsNullOrWhiteSpace(gp.ClassKey))
                                    paps = new PropertyValueAssociationParameterSet(
                                        mirrors[gp.Key],
                                        valueNeuron
                                    );
                                }
                                else
                                    paps = new PropertyInstanceValueAssociationParameterSet(
                                        mirrors[gp.Key],
                                        valueNeuron,
                                        mirrors[gp.ClassKey],
                                        gp.ValueMatchBy
                                    );

                                return paps;
                            })
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
                        mirrors[typeInfo.ValueClassKey],
                        ValueMatchBy.Tag,
                        ivw.Id
                    ),
                    out IInstanceValue instance
                );
            }

            return network;
        }

        // TODO:1 return single values instead of IEnumerable
        public IEnumerable<neurULizationResult<TValue>> DeneurULize<TValue>(
            Network value,
            IEnumerable<Neuron> instanceNeurons,
            neurULizerTypeInfo typeInfo,
            IDictionary<string, Coding.Neuron> mirrors
        )
            where TValue : class, new()
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(instanceNeurons, nameof(instanceNeurons));
            AssertionConcern.AssertArgumentNotNull(typeInfo, nameof(typeInfo));
            AssertionConcern.AssertArgumentNotNull(mirrors, nameof(mirrors));

            var result = new List<neurULizationResult<TValue>>();

            foreach (var instanceNeuron in instanceNeurons)
            {
                bool bResult = false;
                TValue tempResult = default;

                if (!typeof(IInstanceValueWrapper).IsAssignableFrom(typeof(TValue)))
                {
                    if (this.options.InductiveInstanceReader.TryParse(
                        value,
                        new Processors.Readers.Inductive.InstanceParameterSet(
                            instanceNeuron,
                            mirrors[typeInfo.ValueClassKey],
                            typeInfo.GrannyProperties.Select(gp =>
                            {
                                IPropertyAssociationParameterSet paps = null;

                                if (neurULizer.IsValueAssociation(gp))
                                    paps = Processors.Readers.Inductive.PropertyValueAssociationParameterSet.CreateWithoutGranny(
                                        mirrors[gp.Key]
                                    );
                                else
                                    paps = Processors.Readers.Inductive.PropertyInstanceValueAssociationParameterSet.CreateWithoutGranny(
                                        mirrors[gp.Key],
                                        mirrors[gp.ClassKey]
                                    );

                                return paps;
                            }
                            )
                        ),
                        out IInstance instance
                    ))
                    {
                        tempResult = new TValue();

                        foreach (var gp in typeInfo.GrannyProperties)
                        {
                            var propAssoc = instance.PropertyAssociations.SingleOrDefault(
                                pa => pa.HasPropertyAssignment(
                                    this.options.Mirrors.Unit,
                                    // property neuron
                                    mirrors[gp.Key]
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
                                AssertionConcern.Equals(gp.ClassKey, MirrorConfig.ToKeyString(property.PropertyType));

                                string propValueString = propValueGranny.GetValueTag(this.options.Mirrors.NominalSubject.Id);

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
                                    // ei8.Cortex.Coding.Reflection.ReflectionExtensions.ToPropertyData uses "o" format-specifier (RoundtripKind)
                                    propValue = DateTimeOffset.Parse(propValueString, null, DateTimeStyles.RoundtripKind);
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

                        bResult = true;
                    }
                }
                else
                {
                    if (options.InductiveInstanceValueReader.TryParse(
                        value,
                        new Processors.Readers.Inductive.InstanceValueParameterSet(
                            instanceNeuron,
                            mirrors[typeInfo.ValueClassKey]
                        ),
                        out IInstanceValue parseResult
                    ))
                    {
                        var tvw = (IInstanceValueWrapper) new TValue();
                        tvw.Id = parseResult.Neuron.Id;
                        tvw.Tag = parseResult.GetValueTag(this.options.Mirrors.NominalSubject.Id);
                        tempResult = (TValue) tvw;
                        bResult = true;
                    }
                }

                result.Add(new neurULizationResult<TValue>(
                    bResult,
                    instanceNeuron,
                    tempResult
                ));
            }

            return result.AsEnumerable();
        }

        private static bool IsValueAssociation(PropertyData gp)
        {
            return string.IsNullOrWhiteSpace(gp.ClassKey) || 
                gp.ValueMatchBy == ValueMatchBy.Id;
        }

        // TODO:0 remove?
        public IneurULizerOptions Options => this.options;
    }
}
