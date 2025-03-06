using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ValueReader : IValueReader
    {
        private readonly IInstantiatesClassReader instantiatesClassReader;
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;

        public ValueReader(
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
            IValueParameterSet parameters,
            IExternalReferenceSet externalReferences,
            Network network
        ) =>
            new IGreatGrannyInfo<IInstanceValue>[]
            {
                new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstanceValue>(
                    instantiatesClassReader,
                    () => ValueReader.CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IInstanceValue>(
                    expressionReader,
                    (g) => ValueReader.CreateExpressionParameterSet(
                        externalReferences, 
                        parameters, 
                        g.Neuron, 
                        network
                    ),
                    (g, r) => r.Expression = g
                    )
            };

        public IEnumerable<IGrannyQuery> GetQueries(Network network, IValueParameterSet parameters) =>
            parameters.Class != null ?
                new IGrannyQuery[] {
                    new GreatGrannyQuery<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet>(
                        this.instantiatesClassReader,
                        (n) => ValueReader.CreateInstantiatesClassParameterSet(parameters)
                    ),
                    new GreatGrannyQuery<IExpression, IExpressionReader, IExpressionParameterSet>(
                        this.expressionReader,
                        (n) => ValueReader.CreateExpressionParameterSet(this.externalReferences, parameters, n.Last().Neuron, network)
                    )
                } :
                new[] {
                    new GrannyQuery(
                        new NeuronQuery()
                        {
                            Id = new[] { parameters.Value.Id.ToString () }
                        }
                    )
                };

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        private static IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IValueParameterSet parameters,
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

        public bool TryParse(Network network, IValueParameterSet parameters, out IValue result)
        {
            result = null;

            if (parameters.Class != null)
            {
                if (new InstanceValue().AggregateTryParse(
                    ValueReader.CreateGreatGrannies(
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
            }
            else
            {
                result = new Value();
                result.Neuron = parameters.Value;
            }

            return result != null;
        }
    }
}
