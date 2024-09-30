using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyValueExpressionWriter : IPropertyValueExpressionWriter
    {
        private readonly IValueExpressionWriter valueExpressionWriter;
        private readonly IExpressionWriter expressionWriter;
        private readonly Readers.Deductive.IPropertyValueExpressionReader reader;
        private readonly IPrimitiveSet primitives;

        public PropertyValueExpressionWriter(
            IValueExpressionWriter valueExpressionWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyValueExpressionReader reader,
            IPrimitiveSet primitives
            )
        {
            this.valueExpressionWriter = valueExpressionWriter;
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.primitives = primitives;
        }

        public IPropertyValueExpression Build(
            Ensemble ensemble, 
            IPropertyValueExpressionParameterSet parameters
        ) =>
            new PropertyValueExpression().AggregateBuild(
                PropertyValueExpressionWriter.CreateGreatGrannies(
                    this.valueExpressionWriter,
                    this.expressionWriter, 
                    parameters,
                    primitives
                ),
                new IGreatGrannyProcess<IPropertyValueExpression>[]
                {
                    new GreatGrannyProcess<IValueExpression, IValueExpressionWriter, IValueExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.ParseBuild
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.ParseBuild
                        )
                },
                ensemble
            );

        private static IEnumerable<IGreatGrannyInfo<IPropertyValueExpression>> CreateGreatGrannies(
            IValueExpressionWriter valueExpressionWriter,
            IExpressionWriter expressionWriter,
            IPropertyValueExpressionParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
            new IGreatGrannyInfo<IPropertyValueExpression>[]
            {
                new IndependentGreatGrannyInfo<IValueExpression, IValueExpressionWriter, IValueExpressionParameterSet, IPropertyValueExpression>(
                    valueExpressionWriter,
                    () => PropertyValueExpressionWriter.CreateValueExpressionParameterSet(parameters),
                    (g, r) => r.ValueExpression = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyValueExpression>(
                    expressionWriter,
                    (g) => CreateExpressionParameterSet(primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
            };

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IPropertyValueExpressionParameterSet parameters, Neuron neuron) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        neuron,
                        primitives.Unit
                    ),
                    new UnitParameterSet(
                        primitives.Of,
                        primitives.Case
                    )
                }
            );

        private static IValueExpressionParameterSet CreateValueExpressionParameterSet(IPropertyValueExpressionParameterSet parameters) =>
            new ValueExpressionParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        public IGrannyReader<IPropertyValueExpression, IPropertyValueExpressionParameterSet> Reader => this.reader;
    }
}
