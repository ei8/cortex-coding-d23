using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ValueReader : IValueReader
    {
        private readonly IInstantiatesClassReader instantiatesClassReader;

        public ValueReader(IInstantiatesClassReader instantiatesClassReader)
        {
            this.instantiatesClassReader = instantiatesClassReader;
        }

        private static IEnumerable<IGreatGrannyInfo<IInstanceValue>> CreateGreatGrannies(
            IInstantiatesClassReader instantiatesClassReader,
            IValueParameterSet parameters,
            Ensemble ensemble
            ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstanceValue>(
                    instantiatesClassReader,
                    () => ValueReader.CreateInstantiatesClassParameterSet(parameters, gc),
                    (g, r) => r.InstantiatesClass = g
                )
            );

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(
            IValueParameterSet parameters,
            Neuron grannyCandidate
            ) =>
            new InstantiatesClassParameterSet(
                grannyCandidate,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IValueParameterSet parameters, out IValue result)
        {
            var bResult = false;
            result = null;
            IInstanceValue tempIV = null;

            if (parameters.Class != null)
            {
                if (new InstanceValue().AggregateTryParse<IInstanceValue>(
                        parameters.Granny,
                        ValueReader.CreateGreatGrannies(this.instantiatesClassReader, parameters, ensemble),
                        new IGreatGrannyProcess<IInstanceValue>[]
                        {
                        new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstanceValue>(
                            ProcessHelper.TryParse
                        )
                        },
                        ensemble,
                        1,
                        out tempIV
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
