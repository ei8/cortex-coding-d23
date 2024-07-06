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

            string key = GetExternalReferenceKey(value.GetType());
            var contentPropertyKey = GetExternalReferenceKey(value.GetType().GetProperty("Content"));
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

            var propertyAssignment = new PropertyAssignment();
            propertyAssignment = (PropertyAssignment)await propertyAssignment.ObtainAsync(
                result,
                options.Primitives,
                new PropertyAssignmentParameterSet(
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

        private static string GetExternalReferenceKey(MemberInfo value)
        {
            // get ExternalReferenceKeyAttribute of root type
            var erka = value.GetCustomAttributes(typeof(ExternalReferenceKeyAttribute), true).SingleOrDefault() as ExternalReferenceKeyAttribute;
            var key = string.Empty;
            // if attribute exists
            if (erka != null)
                key = erka.Key;
            else if (value is PropertyInfo pi)
                key = pi.ToExternalReferenceKeyString();
            else if (value is Type t)
                // assembly qualified name 
                key = t.ToExternalReferenceKeyString();
            else
                throw new ArgumentOutOfRangeException(nameof(value));
            return key;
        }
    }
}
