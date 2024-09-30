using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyValueExpressionReader : IPropertyValueExpressionReader
    {
        private readonly IValueExpressionReader valueExpressionReader;
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;

        public PropertyValueExpressionReader(IValueExpressionReader valueExpressionReader, IExpressionReader expressionReader, IPrimitiveSet primitives)
        {
            this.valueExpressionReader = valueExpressionReader;
            this.expressionReader = expressionReader;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyValueExpression>> CreateGreatGrannies(
            IValueExpressionReader valueExpressionReader,
            IExpressionReader expressionReader,
            IPropertyValueExpressionParameterSet parameters,
            Ensemble ensemble,
            IPrimitiveSet primitives
        ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyValueExpression>(
                    expressionReader,
                    () => PropertyValueExpressionReader.CreateExpressionParameterSet(primitives, parameters, gc),
                    (g, r) => r.Expression = g
                )
            ).Concat(
                new IGreatGrannyInfo<IPropertyValueExpression>[]
                {
                    new DependentGreatGrannyInfo<IValueExpression, IValueExpressionReader, IValueExpressionParameterSet, IPropertyValueExpression>(
                        valueExpressionReader,
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
                PropertyValueExpressionReader.CreateGreatGrannies(
                    this.valueExpressionReader,
                    this.expressionReader,
                    parameters,
                    ensemble,
                    this.primitives
                    ),
                new IGreatGrannyProcess<IPropertyValueExpression>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IValueExpression, IValueExpressionReader, IValueExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                2,
                out result
            );
    }
}
