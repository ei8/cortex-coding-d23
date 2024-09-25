using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyValueExpressionProcessor : IPropertyValueExpressionProcessor
    {
        private readonly IValueExpressionProcessor valueExpressionProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly IPrimitiveSet primitives;

        public PropertyValueExpressionProcessor(IValueExpressionProcessor valueExpressionProcessor, IExpressionProcessor expressionProcessor, IPrimitiveSet primitives)
        {
            this.valueExpressionProcessor = valueExpressionProcessor;
            this.expressionProcessor = expressionProcessor;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyValueExpression>> CreateGreatGrannies(
            IValueExpressionProcessor valueExpressionProcessor,
            IExpressionProcessor expressionProcessor,
            IPropertyValueExpressionParameterSet parameters,
            Ensemble ensemble,
            IPrimitiveSet primitives
        ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyValueExpression>(
                    expressionProcessor,
                    () => PropertyValueExpressionProcessor.CreateExpressionParameterSet(primitives, parameters, gc),
                    (g, r) => r.Expression = g
                )
            ).Concat(
                new IGreatGrannyInfo<IPropertyValueExpression>[]
                {
                    new DependentGreatGrannyInfo<IValueExpression, IValueExpressionProcessor, IValueExpressionParameterSet, IPropertyValueExpression>(
                        valueExpressionProcessor,
                        g => CreateValueExpressionParameterSet(
                            parameters,
                            ((IExpression) g).Units.GetValueUnitGranniesByTypeId(primitives.Unit.Id).Single().Value
                            ),
                        (g, r) => r.ValueExpression = g
                    )
                }
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IPrimitiveSet primitives,
            IPropertyValueExpressionParameterSet parameters,
            Neuron unitGranny
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        primitives.Unit
                    ),
                    UnitParameterSet.CreateWithValueAndType(
                        primitives.Of,
                        primitives.Case
                    )
                }
            );

        private static IValueExpressionParameterSet CreateValueExpressionParameterSet(IPropertyValueExpressionParameterSet parameters, Neuron value) =>
            new ValueExpressionParameterSet(
                value,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IPropertyValueExpressionParameterSet parameters, out IPropertyValueExpression result) =>
            new PropertyValueExpression().AggregateTryParse(
                parameters.Granny,
                PropertyValueExpressionProcessor.CreateGreatGrannies(
                    valueExpressionProcessor,
                    expressionProcessor,
                    parameters,
                    ensemble,
                    this.primitives
                    ),
                new IGreatGrannyProcess<IPropertyValueExpression>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IValueExpression, IValueExpressionProcessor, IValueExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                2,
                out result
            );
    }
}
