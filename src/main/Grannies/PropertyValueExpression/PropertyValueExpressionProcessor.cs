﻿using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
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

        public async Task<IPropertyValueExpression> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IPropertyValueExpressionParameterSet parameters) =>
            await new PropertyValueExpression().AggregateBuildAsync(
                new IInnerProcess<IPropertyValueExpression>[]
                {
                    new InnerProcess<IValueExpression, IValueExpressionProcessor, IValueExpressionParameterSet, IPropertyValueExpression>(
                        this.valueExpressionProcessor,
                        (g) => PropertyValueExpressionProcessor.CreateValueExpressionParameterSet(parameters),
                        (g, r) => r.ValueExpression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    ),
                    new InnerProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyValueExpression>(
                        this.expressionProcessor,
                        (g) => PropertyValueExpressionProcessor.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            );

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

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, IPropertyValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<IValueExpression, IValueExpressionProcessor, IValueExpressionParameterSet>(
                    this.valueExpressionProcessor,
                    (n) => PropertyValueExpressionProcessor.CreateValueExpressionParameterSet(parameters)
                ),
                new GrannyQueryInner<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    this.expressionProcessor,
                    (n) => PropertyValueExpressionProcessor.CreateExpressionParameterSet(options.Primitives, parameters, n)
                )
            };

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IPropertyValueExpressionParameterSet parameters, out IPropertyValueExpression result) =>
            (result = new PropertyValueExpression().AggregateTryParse(
                new IInnerProcess<IPropertyValueExpression>[]
                {
                    new InnerProcess<IValueExpression, IValueExpressionProcessor, IValueExpressionParameterSet, IPropertyValueExpression>(
                        this.valueExpressionProcessor,
                        (g) => PropertyValueExpressionProcessor.CreateValueExpressionParameterSet(parameters),
                        (g, r) => r.ValueExpression = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyValueExpression>(
                        this.expressionProcessor,
                        (g) => PropertyValueExpressionProcessor.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            )) != null;
    }
}
