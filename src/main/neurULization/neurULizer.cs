using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class neurULizer : IneurULizer
    {
        public async Task<Ensemble> neurULizeAsync<TValue>(TValue value, IneurULizationOptions options)
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(options, nameof(options));
            AssertionConcern.AssertArgumentValid(
                o => o is neurULizationOptions, 
                options, 
                $"Specified 'options' is not of the expected type '{typeof(neurULizationOptions).FullName}'.", 
                nameof(options)
                );

            var result = new Ensemble();

            var granny = Neuron.CreateTransient(null, null, null);
            result.AddReplace(granny);

            string key = value.GetType().GetExternalReferenceKey();
            var contentPropertyKey = value.GetType().GetProperty("Content").GetExternalReferenceKey();
            // use key to retrieve external reference url from library
            var erDict = await options.ToInternal().EnsembleRepository.GetExternalReferencesAsync(
                options.ToInternal().UserId,
                new string[] {
                    key,
                    contentPropertyKey
                });
            var rootTypeNeuron = erDict[key];

            // instantiates
            var instantiatesClass = new InstantiatesClass();
            instantiatesClass = (InstantiatesClass)await instantiatesClass.ObtainAsync(
                result,
                options.ToInternal(),
                new InstantiatesClassParameterSet(
                    rootTypeNeuron
                    )
                );

            // link granny neuron to InstantiatesType neuron
            result.AddReplace(Terminal.CreateTransient(granny.Id, instantiatesClass.Neuron.Id));

            // DEL: test code
            var idea = Neuron.CreateTransient("Test Idea Expression", null, null);

            await new PropertyAssociation().ObtainAsync(
                result,
                options.ToInternal(),
                new PropertyAssociationParameterSet(
                    erDict[contentPropertyKey],
                    idea,
                    options.ToInternal().Primitives.Idea,
                    ValueMatchByValue.Tag
                    )
                );

            return result;
        }
    }
}
