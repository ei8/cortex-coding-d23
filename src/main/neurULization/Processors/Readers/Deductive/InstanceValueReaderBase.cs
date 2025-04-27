using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public abstract class InstanceValueReaderBase<TInstanceValueParameterSet> :
        LesserExpressionReaderBase<
            IInstantiatesClass,
            IInstantiatesClassParameterSet,
            IInstantiatesClassReader,
            IInstanceValue,
            TInstanceValueParameterSet,
            InstanceValue
        >
        where TInstanceValueParameterSet : IInstanceValueParameterSet
    {
        public InstanceValueReaderBase(
            IInstantiatesClassReader greatGrannyReader,
            IExpressionReader expressionReader,
            IExternalReferenceSet externalReferences
        ) : base(
            greatGrannyReader,
            expressionReader,
            externalReferences
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            TInstanceValueParameterSet parameters,
            Neuron instantiatesClassNeuron,
            Network network
        ) => CreateExpressionParameterSetCore(
            externalReferences,
            parameters,
            instantiatesClassNeuron,
            network
        );

        private static IExpressionParameterSet CreateExpressionParameterSetCore(IExternalReferenceSet externalReferences, IInstanceValueParameterSet parameters, Neuron instantiatesClassNeuron, Network network)
        {
            Neuron valueNeuron;
            if (parameters.ValueMatchBy == ValueMatchBy.Id)
                valueNeuron = parameters.Value;
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

        protected override IInstantiatesClassParameterSet CreateGreatGrannyParameterSet(TInstanceValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );
    }
}
