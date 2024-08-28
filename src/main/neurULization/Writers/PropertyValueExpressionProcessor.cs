using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class PropertyValueExpressionProcessor : IPropertyValueExpressionProcessor
    {
        private readonly IValueExpressionProcessor valueExpressionProcessor;
        private readonly IExpressionProcessor expressionProcessor;

        public PropertyValueExpressionProcessor(IValueExpressionProcessor valueExpressionProcessor, IExpressionProcessor expressionProcessor)
        {
            this.valueExpressionProcessor = valueExpressionProcessor;
            this.expressionProcessor = expressionProcessor;
        }

        public async Task<IPropertyValueExpression> BuildAsync(Ensemble ensemble, Id23neurULizerWriteOptions options, IPropertyValueExpressionParameterSet parameters) =>
            await new PropertyValueExpression().AggregateBuildAsync(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcessAsync<IPropertyValueExpression>[]
                {
                    new GreatGrannyProcessAsync<IValueExpression, IValueExpressionProcessor, IValueExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        ),
                    new GreatGrannyProcessAsync<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                },
                ensemble,
                options
            );

        private IEnumerable<IGreatGrannyInfo<IPropertyValueExpression>> CreateGreatGrannies(Id23neurULizerWriteOptions options, IPropertyValueExpressionParameterSet parameters) =>
            new IGreatGrannyInfo<IPropertyValueExpression>[]
            {
                new IndependentGreatGrannyInfo<IValueExpression, IValueExpressionProcessor, IValueExpressionParameterSet, IPropertyValueExpression>(
                    valueExpressionProcessor,
                    () => CreateValueExpressionParameterSet(parameters),
                    (g, r) => r.ValueExpression = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyValueExpression>(
                    expressionProcessor,
                    (g) => CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
            };

        private static ExpressionParameterSet CreateExpressionParameterSet(PrimitiveSet primitives, IPropertyValueExpressionParameterSet parameters, Neuron neuron) =>
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

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerWriteOptions options, IPropertyValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<IValueExpression, IValueExpressionProcessor, IValueExpressionParameterSet>(
                    valueExpressionProcessor,
                    (n) => CreateValueExpressionParameterSet(parameters)
                ),
                new GrannyQueryInner<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    expressionProcessor,
                    (n) => CreateExpressionParameterSet(options.Primitives, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Ensemble ensemble, Id23neurULizerWriteOptions options, IPropertyValueExpressionParameterSet parameters, out IPropertyValueExpression result) =>
            new PropertyValueExpression().AggregateTryParse(
                CreateGreatGrannies(options, parameters),
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
                options,
                out result
            );
    }
}
