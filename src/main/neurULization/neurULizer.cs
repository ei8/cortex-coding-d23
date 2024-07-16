using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class neurULizer : IneurULizer
    {
        public async Task<Ensemble> neurULizeAsync<TValue>(TValue value, IneurULizerOptions options)
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(options, nameof(options));
            AssertionConcern.AssertArgumentValid(
                o => o is Id23neurULizerOptions, 
                options, 
                $"Specified 'options' is not of the expected type '{typeof(Id23neurULizerOptions).FullName}'.", 
                nameof(options)
                );

            var result = new Ensemble();
            var d23Options = (Id23neurULizerOptions)options;
            string valueClassKey = value.GetType().GetExternalReferenceKey();
            
            var propertyData = value.GetType().GetProperties()
                .Select(pi => pi.ToPropertyData(value))
                .Where(pd => pd != null);

            var neuronProperties = propertyData.Where(pd => pd.NeuronProperty != null).Select(pd => pd.NeuronProperty);
            var grannyProperties = propertyData.Where(pd => pd.NeuronProperty == null);
            Guid? regionId = neuronProperties.OfType<RegionIdProperty>().SingleOrDefault()?.Value;

            IdProperty idp = null;
            var granny = Neuron.CreateTransient(
                (idp = neuronProperties.OfType<IdProperty>().SingleOrDefault()) != null ? 
                    idp.Value :
                    Guid.NewGuid(),
                neuronProperties.OfType<TagProperty>().SingleOrDefault()?.Value,
                neuronProperties.OfType<ExternalReferenceUrlProperty>().SingleOrDefault()?.Value,
                regionId
                );
            result.AddReplace(granny);

            var propertyKeys = grannyProperties.Select(gp => gp.Key)
                .Concat(grannyProperties.Select(gp => gp.ClassKey))
                .Distinct();

            // use key to retrieve external reference url from library
            var externalReferences = await d23Options.EnsembleRepository.GetExternalReferencesAsync(
                d23Options.UserId,
                new string[] {
                    valueClassKey
                }.Concat(
                    propertyKeys
                ).ToArray());

            // instantiates
            var instantiatesClass = await new InstantiatesClass().ObtainAsync(
                result,
                d23Options,
                new InstantiatesClassParameterSet(externalReferences[valueClassKey])
                );

            // link granny neuron to InstantiatesClass neuron
            result.AddReplace(Terminal.CreateTransient(granny.Id, instantiatesClass.Neuron.Id));

            foreach(var gp in grannyProperties)
            {
                // Unnecessary to validate null id and tag values since another service can be
                // responsible for pruning grannies containing null or empty values.
                // Null values can also be considered as valid new values.

                // createtransient pass gp.value to either id or tag of neuron based on valuematchby
                var propertyValue = gp.ValueMatchBy == ValueMatchBy.Id ?
                    Neuron.CreateTransient(Guid.Parse(gp.Value), null, null, regionId) :
                    Neuron.CreateTransient(gp.Value, null, regionId);

                var propertyAssociation = await new PropertyAssociation().ObtainAsync(
                    result,
                    d23Options,
                    new PropertyAssociationParameterSet(
                        externalReferences[gp.Key],
                        propertyValue,
                        externalReferences[gp.ClassKey],
                        ValueMatchBy.Tag
                        )
                    );

                // link granny neuron to PropertyAssociation neuron
                result.AddReplace(Terminal.CreateTransient(granny.Id, propertyAssociation.Neuron.Id));
            }

            return result;
        }
    }
}
