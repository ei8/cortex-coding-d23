using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ValueWriter : IValueWriter
    {
        private readonly IInstantiatesClassWriter instantiatesClassWriter;
        private readonly IExpressionWriter expressionWriter;
        private readonly Readers.Deductive.IValueReader reader;
        private readonly IExternalReferenceSet externalReferences;

        public ValueWriter(
            IInstantiatesClassWriter instantiatesClassWriter,
            IExpressionWriter expressionWriter,
            Readers.Deductive.IValueReader reader,
            IExternalReferenceSet externalReferences
        )
        {
            this.instantiatesClassWriter = instantiatesClassWriter;
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.externalReferences = externalReferences;
        }

        public IValue Build(Network network, IValueParameterSet parameters) =>
            parameters.Class != null ?
                (IValue) new InstanceValue().AggregateBuild(
                    ValueWriter.CreateGreatGrannies(
                        this.instantiatesClassWriter,
                        this.expressionWriter,
                        parameters,
                        this.externalReferences
                    ),
                    new IGreatGrannyProcess<IInstanceValue>[]
                    {
                        new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassWriter, IInstantiatesClassParameterSet, IInstanceValue>(
                            ProcessHelper.ParseBuild
                        ),
                        new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, IInstanceValue>(
                            ProcessHelper.ParseBuild
                        )
                    },
                    network
                ) :
                new Value()
                {
                    Neuron = network.AddOrGetIfExists(parameters.Value)
                };

        private static IEnumerable<IGreatGrannyInfo<IInstanceValue>> CreateGreatGrannies(
            IInstantiatesClassWriter instantiatesClassWriter,
            IExpressionWriter expressionWriter,
            IValueParameterSet parameters,
            IExternalReferenceSet externalReferences
        ) =>
            new IGreatGrannyInfo<IInstanceValue>[]
            {
                new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassWriter, IInstantiatesClassParameterSet, IInstanceValue>(
                    instantiatesClassWriter,
                    () => ValueWriter.CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IInstanceValue>(
                    expressionWriter,
                    (g) => ValueWriter.CreateExpressionParameterSet(externalReferences, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
            };

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IValueParameterSet parameters, Neuron instantiatesClassNeuron) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        instantiatesClassNeuron,
                        externalReferences.Unit
                    ),
                    new UnitParameterSet(
                        parameters.Value,
                        externalReferences.NominalSubject
                    )
                }
            );

        public IGrannyReader<IValue, IValueParameterSet> Reader => this.reader;
    }
}
