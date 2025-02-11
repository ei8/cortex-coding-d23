using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ValueReader : IValueReader
    {
        private readonly IInstantiatesClassReader instantiatesClassReader;
        private readonly IAggregateParser aggregateParser;

        public ValueReader(IInstantiatesClassReader instantiatesClassReader, IAggregateParser aggregateParser)
        {
            AssertionConcern.AssertArgumentNotNull(instantiatesClassReader, nameof(instantiatesClassReader));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.instantiatesClassReader = instantiatesClassReader;
            this.aggregateParser = aggregateParser;
        }

        private static IGreatGrannyInfoSuperset<IInstanceValue> CreateGreatGrannies(
            IInstantiatesClassReader instantiatesClassReader,
            IValueParameterSet parameters,
            Network network
            ) =>
            ProcessHelper.CreateGreatGrannyCandidateSet(
                network,
                parameters.Granny,
                gc => new InductiveIndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstanceValue>(
                    gc,
                    instantiatesClassReader,
                    () => ValueReader.CreateInstantiatesClassParameterSet(parameters, gc),
                    (g, r) => r.InstantiatesClass = g
                ),
                new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstanceValue>(
                    ProcessHelper.TryParse
                )
            ).AsSuperset();

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(
            IValueParameterSet parameters,
            Neuron grannyCandidate
            ) =>
            new InstantiatesClassParameterSet(
                grannyCandidate,
                parameters.Class
            );

        public bool TryParse(Network network, IValueParameterSet parameters, out IValue result)
        {
            var bResult = false;
            result = null;

            if (parameters.Class != null)
            {
                if (this.aggregateParser.TryParse<InstanceValue, IInstanceValue>(
                        parameters.Granny,
                        ValueReader.CreateGreatGrannies(this.instantiatesClassReader, parameters, network),
                        network,
                        out IInstanceValue tempIV
                    )
                )
                {
                    bResult = true;
                    result = tempIV;
                }
            }
            else
            {
                bResult = true;
                result = new Value();
                result.Neuron = parameters.Granny;
            }

            return bResult;
        }
    }
}
