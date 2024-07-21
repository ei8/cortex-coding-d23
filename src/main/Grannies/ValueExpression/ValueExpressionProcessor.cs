﻿using ei8.Cortex.Coding.d23.neurULization;
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
                this.CreateInnerProcesses(options, parameters),
                new IInnerProcessTargetAsync<IValueExpression>[]
                {
                    new InnerProcessTargetAsync<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        ),
                    new InnerProcessTargetAsync<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                },
                ensemble,
                options
            );

        private IEnumerable<IInnerProcess<IValueExpression>> CreateInnerProcesses(Id23neurULizerOptions options, IValueExpressionParameterSet parameters) =>
            new IInnerProcess<IValueExpression>[]
            {
                new InnerProcess<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                    this.valueProcessor,
                    (g) => ValueExpressionProcessor.CreateValueParameterSet(parameters),
                    (g, r) => r.Value = g
                    ),
                new InnerProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
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
                this.CreateInnerProcesses(options, parameters),
                new IInnerProcessTarget<IValueExpression>[]
                {
                    new InnerProcessTarget<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new InnerProcessTarget<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                out result
            );
    }
}
