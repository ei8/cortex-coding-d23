using ei8.Cortex.Coding.d23.Grannies;
using Microsoft.Extensions.DependencyInjection;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

            string valueClassKey = value.GetType().GetExternalReferenceKey();

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
            var externalReferences = await options.ServiceProvider.GetRequiredService<IEnsembleRepository>()
                .GetExternalReferencesAsync(
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
                .ObtainAsync<IInstance, IInstanceProcessor, IInstanceParameterSet>(
                    result,
                    options,
                    new InstanceParameterSet(
                        (idp = neuronProperties.OfType<IdProperty>().SingleOrDefault()) != null ?
                            idp.Value :
                            Guid.NewGuid(),
                        neuronProperties.OfType<TagProperty>().SingleOrDefault()?.Value,
                        neuronProperties.OfType<ExternalReferenceUrlProperty>().SingleOrDefault()?.Value,
                        regionId,
                        externalReferences[valueClassKey],
                        grannyProperties.Select(gp =>
                            new PropertyAssociationParameterSet(
                                externalReferences[gp.Key],
                                (
                                    gp.ValueMatchBy == ValueMatchBy.Id ?
                                        Neuron.CreateTransient(Guid.Parse(gp.Value), null, null, regionId) :
                                        Neuron.CreateTransient(gp.Value, null, regionId)
                                ),
                                externalReferences[gp.ClassKey],
                                gp.ValueMatchBy
                            )
                        ),
                        options.OperationOptions.Mode
                    )
                );

            return result;
        }

        public static IEnumerable<TValue> DeneurULize<TValue>(this neurULizer neurULizer, Ensemble value, Id23neurULizerReadOptions options)
            where TValue : class, new()
        {
            TValue result = new TValue();

            // get properties
            var propertyData = value.GetType().GetProperties()
                .Select(pi => pi.ToPropertyData(value))
                .Where(pd => pd != null);

            var neuronProperties = propertyData.Where(pd => pd.NeuronProperty != null).Select(pd => pd.NeuronProperty);
            var grannyProperties = propertyData.Where(pd => pd.NeuronProperty == null);

            //options.ServiceProvider.GetRequiredService<IInstanceProcessor>().TryParse(
            //    value,
            //    options,
            //    new InstanceParameterSet(
            //        ))


            return new TValue[] { result };
        }
    }
}
