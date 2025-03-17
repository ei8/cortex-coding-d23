using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class InstanceValueWriter : IInstanceValueWriter
    {
        private readonly IInstantiatesClassWriter instantiatesClassWriter;
        private readonly IExpressionWriter expressionWriter;
        private readonly Readers.Deductive.IInstanceValueReader reader;
        private readonly IExternalReferenceSet externalReferences;

        public InstanceValueWriter(
            IInstantiatesClassWriter instantiatesClassWriter,
            IExpressionWriter expressionWriter,
            Readers.Deductive.IInstanceValueReader reader,
            IExternalReferenceSet externalReferences
        )
        {
            this.instantiatesClassWriter = instantiatesClassWriter;
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.externalReferences = externalReferences;
        }

        public IInstanceValue Build(Network network, IInstanceValueParameterSet parameters) =>
            new InstanceValue().AggregateBuild(
                InstanceValueWriter.CreateGreatGrannies(
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
            );

        private static IEnumerable<IGreatGrannyInfo<IInstanceValue>> CreateGreatGrannies(
                IInstantiatesClassWriter instantiatesClassWriter,
                IExpressionWriter expressionWriter,
                IInstanceValueParameterSet parameters,
                IExternalReferenceSet externalReferences
            ) =>
                new IGreatGrannyInfo<IInstanceValue>[]
                {
                    new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassWriter, IInstantiatesClassParameterSet, IInstanceValue>(
                        instantiatesClassWriter,
                        () => InstanceValueWriter.CreateInstantiatesClassParameterSet(parameters),
                        (g, r) => r.InstantiatesClass = g
                    ),
                    new DependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IInstanceValue>(
                        expressionWriter,
                        (g) => InstanceValueWriter.CreateExpressionParameterSet(externalReferences, parameters, g.Neuron),
                        (g, r) => r.Expression = g
                    )
                };

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IInstanceValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IInstanceValueParameterSet parameters, Neuron instantiatesClassNeuron) =>
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

        public IGrannyReader<IInstanceValue, IInstanceValueParameterSet> Reader => this.reader;
    }
}
