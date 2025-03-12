using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstanceValueReader : IInstanceValueReader
    {
        private readonly IInstantiatesClassReader instantiatesClassReader;
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;

        public InstanceValueReader(
            IInstantiatesClassReader instantiatesClassReader,
            IExpressionReader expressionReader,
            IExternalReferenceSet externalReferences
        )
        {
            this.instantiatesClassReader = instantiatesClassReader;
            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
        }

        private static IEnumerable<IGreatGrannyInfo<IInstanceValue>> CreateGreatGrannies(
            IInstantiatesClassReader instantiatesClassReader,
            IExpressionReader expressionReader,
            IInstanceValueParameterSet parameters,
            IExternalReferenceSet externalReferences,
            Network network
        ) =>
            new IGreatGrannyInfo<IInstanceValue>[]
            {
                new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstanceValue>(
                    instantiatesClassReader,
                    () => InstanceValueReader.CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IInstanceValue>(
                    expressionReader,
                    (g) => InstanceValueReader.CreateExpressionParameterSet(
                        externalReferences,
                        parameters,
                        g.Neuron,
                        network
                    ),
                    (g, r) => r.Expression = g
                    )
            };

        public IEnumerable<IGrannyQuery> GetQueries(
            Network network,
            IInstanceValueParameterSet parameters
        ) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet>(
                    this.instantiatesClassReader,
                    (n) => InstanceValueReader.CreateInstantiatesClassParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionReader, IExpressionParameterSet>(
                    this.expressionReader,
                    (n) => InstanceValueReader.CreateExpressionParameterSet(this.externalReferences, parameters, n.Last().Neuron, network)
                )
            };

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IInstanceValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        private static IExpressionParameterSet CreateExpressionParameterSet(
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

            return new ExpressionParameterSet(
                new[] {
                    new UnitParameterSet(
                        instantiatesClassNeuron,
                        externalReferences.Unit
                    ),
                    new UnitParameterSet(
                        valueNeuron,
                        externalReferences.NominalSubject
                    )
                }
            );
        }

        public bool TryParse(Network network, IInstanceValueParameterSet parameters, out IInstanceValue result)
        {
            result = null;

            if (new InstanceValue().AggregateTryParse(
                InstanceValueReader.CreateGreatGrannies(
                    this.instantiatesClassReader,
                    this.expressionReader,
                    parameters,
                    this.externalReferences,
                    network
                ),
                new IGreatGrannyProcess<IInstanceValue>[]
                {
                        new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstanceValue>(
                            ProcessHelper.TryParse
                        ),
                        new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IInstanceValue>(
                            ProcessHelper.TryParse
                        )
                },
                network,
                out IInstanceValue tempResult
            ))
                result = tempResult;

            return result != null;
        }
    }
}
