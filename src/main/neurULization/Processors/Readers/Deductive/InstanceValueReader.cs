using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstanceValueReader : 
        ExpressionReaderBase<
            IInstantiatesClass,
            IInstantiatesClassParameterSet,
            IInstantiatesClassReader,
            IInstanceValue,
            IInstanceValueParameterSet,
            InstanceValue
        >,
        IInstanceValueReader
    {
        public InstanceValueReader(
            IInstantiatesClassReader greatGrannyReader,
            IExpressionReader expressionReader,
            IExternalReferenceSet externalReferences
        ) : base (
            greatGrannyReader, 
            expressionReader, 
            externalReferences
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IInstanceValueParameterSet parameters,
            Neuron instantiatesClassNeuron,
            Network network
        )
        {
            Neuron valueNeuron;
            if (parameters.ValueMatchBy == ValueMatchBy.Id)
                network.TryGetById<Neuron>(parameters.Value.Id, out valueNeuron);
            else
            {
                network.TryGetByTag(parameters.Value.Tag, out IEnumerable<Neuron> results);
                valueNeuron = results.SingleOrDefault();
                Trace.WriteLineIf(results.Count() > 1, $"Multiple neurons matched while parsing InstanceValue with tag '{parameters.Value.Tag}'");
            }

            return ProcessorExtensions.CreateInstanceValueParameterSet(
                externalReferences,
                valueNeuron,
                instantiatesClassNeuron
            );
        }

        protected override IInstantiatesClassParameterSet CreateGreatGrannyParameterSet(IInstanceValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );
    }
}
