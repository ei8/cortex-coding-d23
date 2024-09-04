using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyValueExpressionProcessor : IPropertyValueExpressionProcessor
    {
        private readonly IValueExpressionProcessor valueExpressionProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly Readers.Deductive.IPropertyValueExpressionProcessor readerProcessor;

        public PropertyValueExpressionProcessor(
            IValueExpressionProcessor valueExpressionProcessor, 
            IExpressionProcessor expressionProcessor,
            Readers.Deductive.IPropertyValueExpressionProcessor readerProcessor
            )
        {
            this.valueExpressionProcessor = valueExpressionProcessor;
            this.expressionProcessor = expressionProcessor;
            this.readerProcessor=readerProcessor;
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

        public IGrannyReadProcessor<IPropertyValueExpression, IPropertyValueExpressionParameterSet> ReadProcessor => this.readerProcessor;
    }
}
