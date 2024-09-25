using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyValueExpressionProcessor : IPropertyValueExpressionProcessor
    {
        private readonly IValueExpressionProcessor valueExpressionProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly IPrimitiveSet primitives;

        public PropertyValueExpressionProcessor(
            IValueExpressionProcessor valueExpressionProcessor, 
            IExpressionProcessor expressionProcessor, 
            IPrimitiveSet primitives
        )
        {
            this.valueExpressionProcessor = valueExpressionProcessor;
            this.expressionProcessor = expressionProcessor;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyValueExpression>> CreateGreatGrannies(
            IValueExpressionProcessor valueExpressionProcessor,
            IExpressionProcessor expressionProcessor,
            IPropertyValueExpressionParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
            new IGreatGrannyInfo<IPropertyValueExpression>[]
            {
                new IndependentGreatGrannyInfo<IValueExpression, IValueExpressionProcessor, IValueExpressionParameterSet, IPropertyValueExpression>(
                    valueExpressionProcessor,
                    () => PropertyValueExpressionProcessor.CreateValueExpressionParameterSet(parameters),
                    (g, r) => r.ValueExpression = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyValueExpression>(
                    expressionProcessor,
                    (g) => PropertyValueExpressionProcessor.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
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

        public IEnumerable<IGrannyQuery> GetQueries(IPropertyValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IValueExpression, IValueExpressionProcessor, IValueExpressionParameterSet>(
                    valueExpressionProcessor,
                    (n) => PropertyValueExpressionProcessor.CreateValueExpressionParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    expressionProcessor,
                    (n) => PropertyValueExpressionProcessor.CreateExpressionParameterSet(this.primitives, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Ensemble ensemble, IPropertyValueExpressionParameterSet parameters, out IPropertyValueExpression result) =>
            new PropertyValueExpression().AggregateTryParse(
                PropertyValueExpressionProcessor.CreateGreatGrannies(
                    this.valueExpressionProcessor,
                    this.expressionProcessor,
                    parameters,
                    this.primitives
                ),
                new IGreatGrannyProcess<IPropertyValueExpression>[]
                {
                    new GreatGrannyProcess<IValueExpression, IValueExpressionProcessor, IValueExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                out result
            );
    }
}
