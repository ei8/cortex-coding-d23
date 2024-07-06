using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class neurULizer
    {
        public async Task<Ensemble> neurULizeAsync<TValue>(TValue value, neurULizationOptions options)
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(options, nameof(options));

            var result = new Ensemble();

            var granny = Neuron.CreateTransient(null, null, null);
            result.AddReplace(granny);

            string key = value.GetType().GetExternalReferenceKey();
            var contentPropertyKey = value.GetType().GetProperty("Content").GetExternalReferenceKey();
            // use key to retrieve external reference url from library
            var erDict = await options.EnsembleRepository.GetExternalReferencesAsync(
                options.UserId,
                new string[] {
                    key,
                    contentPropertyKey
                });
            var rootTypeNeuron = erDict[key];

            // instantiates
            var instantiatesClass = new InstantiatesClass();
            instantiatesClass = (InstantiatesClass)await instantiatesClass.ObtainAsync(
                result,
                options.Primitives,
                new InstantiatesClassParameterSet(
                    rootTypeNeuron,
                    options.EnsembleRepository,
                    options.UserId
                    )
                );

            // link granny neuron to InstantiatesType neuron
            result.AddReplace(Terminal.CreateTransient(granny.Id, instantiatesClass.Neuron.Id));

            // DEL: test code
            var idea = Neuron.CreateTransient("Test Idea Expression", null, null);

            await new PropertyAssociation().ObtainAsync(
                result,
                options.Primitives,
                new PropertyAssociationParameterSet(
                    erDict[contentPropertyKey],
                    idea,
                    options.Primitives.Idea,
                    ValueMatchByValue.Tag,
                    options.EnsembleRepository,
                    options.UserId
                    )
                );

            return result;
        }
    }
}
