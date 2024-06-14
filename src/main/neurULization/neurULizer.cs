using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Linq;
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

            // get ExternalReferenceKeyAttribute of root type
            var erka = value.GetType().GetCustomAttributes(typeof(ExternalReferenceKeyAttribute), true).SingleOrDefault() as ExternalReferenceKeyAttribute;
            var key = string.Empty;
            // if attribute exists
            if (erka != null)
                key = erka.Key;
            else
                // assembly qualified name 
                key = value.GetType().ToExternalReferenceKeyString();
            // use key to retrieve external reference url from library
            var erDict = await options.EnsembleRepository.GetExternalReferencesAsync(options.UserId, new string[] { key });
            var rootTypeNeuron = erDict[key];

            // instantiates
            var instantiatesClass = new InstantiatesClass();
            instantiatesClass = (InstantiatesClass) await instantiatesClass.ObtainAsync(
                result,
                options.Primitives,
                new InstantiatesClassParameterSet(
                    rootTypeNeuron,
                    options.EnsembleRepository,
                    options.UserId
                    ),
                options.EnsembleRepository,
                options.UserId
                );

            // link granny neuron to InstantiatesType neuron
            result.AddReplace(Terminal.CreateTransient(granny.Id, instantiatesClass.Neuron.Id));

            // DEL: test code
            var idea = Neuron.CreateTransient("Test Idea", null, null);
            var instantiation = new Instantiation();
            instantiation = (Instantiation)await instantiation.ObtainAsync(
                result,
                options.Primitives,
                new InstantiationParameterSet(
                    idea,
                    options.Primitives.Idea,
                    InstantiationMatchingNeuronProperty.Tag,
                    options.EnsembleRepository,
                    options.UserId
                    ),
                options.EnsembleRepository,
                options.UserId
                );


            return result;
        }
    }
}
