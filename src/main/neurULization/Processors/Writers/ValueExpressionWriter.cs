using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ValueExpressionWriter : IValueExpressionWriter
    {
        private readonly IValueWriter valueWriter;
        private readonly IExpressionWriter expressionWriter;
        private readonly Readers.Deductive.IValueExpressionReader reader;
        private readonly IExternalReferenceSet externalReferences;

        public ValueExpressionWriter(
            IValueWriter valueWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IValueExpressionReader reader,
            IExternalReferenceSet externalReferences
            )
        {
            this.valueWriter = valueWriter;
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.externalReferences = externalReferences;
        }

        public IValueExpression Build(Network network, IValueExpressionParameterSet parameters) =>
            new ValueExpression().AggregateBuild(
                ValueExpressionWriter.CreateGreatGrannies(
                    this.valueWriter,
                    this.expressionWriter,
                    parameters,
                    this.externalReferences
                ),
                new IGreatGrannyProcess<IValueExpression>[]
                {
                    new GreatGrannyProcess<IValue, IValueWriter, IValueParameterSet, IValueExpression>(
                        ProcessHelper.ParseBuild
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.ParseBuild
                        )
                },
                network
            );

        private static IEnumerable<IGreatGrannyInfo<IValueExpression>> CreateGreatGrannies(
            IValueWriter valueWriter,
            IExpressionWriter expressionWriter, 
            IValueExpressionParameterSet parameters,
            IExternalReferenceSet externalReferences
        ) =>
            new IGreatGrannyInfo<IValueExpression>[]
            {
                new IndependentGreatGrannyInfo<IValue, IValueWriter, IValueParameterSet, IValueExpression>(
                    valueWriter,
                    () => ValueExpressionWriter.CreateValueParameterSet(parameters),
                    (g, r) => r.Value = g
                    ),
                new DependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IValueExpression>(
                    expressionWriter,
                    (g) => ValueExpressionWriter.CreateExpressionParameterSet(externalReferences, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                    )
            };

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IValueExpressionParameterSet parameters, Neuron n) =>
            new ExpressionParameterSet(
                new[] {
                    new UnitParameterSet(
                        n,
                        externalReferences.Unit
                    ),
                }
            );

        public IGrannyReader<IValueExpression, IValueExpressionParameterSet> Reader => this.reader;
    }
}
