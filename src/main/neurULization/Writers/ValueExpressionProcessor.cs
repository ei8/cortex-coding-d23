using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class ValueExpressionProcessor : IValueExpressionProcessor
    {
        private readonly IValueProcessor valueProcessor;
        private readonly IExpressionProcessor expressionProcessor;

        public ValueExpressionProcessor(IValueProcessor valueProcessor, IExpressionProcessor expressionProcessor)
        {
            this.valueProcessor = valueProcessor;
            this.expressionProcessor = expressionProcessor;
        }

        public async Task<IValueExpression> BuildAsync(Ensemble ensemble, Id23neurULizerWriteOptions options, IValueExpressionParameterSet parameters) =>
            await new ValueExpression().AggregateBuildAsync(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyWriteProcessAsync<IValueExpression>[]
                {
                    new GreatGrannyWriteProcessAsync<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        ),
                    new GreatGrannyWriteProcessAsync<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                },
                ensemble,
                options
            );

        private IEnumerable<IGreatGrannyInfo<IValueExpression>> CreateGreatGrannies(Id23neurULizerWriteOptions options, IValueExpressionParameterSet parameters) =>
            new IGreatGrannyInfo<IValueExpression>[]
            {
                new GreatGrannyWriteInfo<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                    valueProcessor,
                    (g) => CreateValueParameterSet(parameters),
                    (g, r) => r.Value = g
                    ),
                new GreatGrannyWriteInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                    expressionProcessor,
                    (g) => CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                    )
            };

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerWriteOptions options, IValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<IValue, IValueProcessor, IValueParameterSet>(
                    valueProcessor,
                    (n) => CreateValueParameterSet(parameters)
                ),
                new GrannyQueryInner<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    expressionProcessor,
                    (n) => CreateExpressionParameterSet(options.Primitives, parameters, n.Last().Neuron)
                )
            };

        private static ExpressionParameterSet CreateExpressionParameterSet(PrimitiveSet primitives, IValueExpressionParameterSet parameters, Neuron n) =>
            new ExpressionParameterSet(
                new[] {
                    new UnitParameterSet(
                        n,
                        primitives.Unit
                    ),
                }
            );

        public bool TryParse(Ensemble ensemble, Id23neurULizerWriteOptions options, IValueExpressionParameterSet parameters, out IValueExpression result) =>
            new ValueExpression().AggregateTryParse(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyWriteProcess<IValueExpression>[]
                {
                    new GreatGrannyWriteProcess<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyWriteProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                out result
            );
    }
}
