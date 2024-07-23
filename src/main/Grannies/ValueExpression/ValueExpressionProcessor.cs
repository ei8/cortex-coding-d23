using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
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

        public async Task<IValueExpression> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IValueExpressionParameterSet parameters) =>
            await new ValueExpression().AggregateBuildAsync(
                this.CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcessAsync<IValueExpression>[]
                {
                    new GreatGrannyProcessAsync<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        ),
                    new GreatGrannyProcessAsync<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                },
                ensemble,
                options
            );

        private IEnumerable<IGreatGrannyInfo<IValueExpression>> CreateGreatGrannies(Id23neurULizerOptions options, IValueExpressionParameterSet parameters) =>
            new IGreatGrannyInfo<IValueExpression>[]
            {
                new GreatGrannyInfo<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                    this.valueProcessor,
                    (g) => ValueExpressionProcessor.CreateValueParameterSet(parameters),
                    (g, r) => r.Value = g
                    ),
                new GreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                    this.expressionProcessor,
                    (g) => ValueExpressionProcessor.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                    )
            };

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, IValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<IValue, IValueProcessor, IValueParameterSet>(
                    this.valueProcessor,
                    (n) => ValueExpressionProcessor.CreateValueParameterSet(parameters)
                ),
                new GrannyQueryInner<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    this.expressionProcessor,
                    (n) => ValueExpressionProcessor.CreateExpressionParameterSet(options.Primitives, parameters, n.Last().Neuron)
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

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IValueExpressionParameterSet parameters, out IValueExpression result) =>
            new ValueExpression().AggregateTryParse(
                this.CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcess<IValueExpression>[]
                {
                    new GreatGrannyProcess<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                out result
            );
    }
}
