using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyAssignmentProcessor : IPropertyAssignmentProcessor
    {
        private readonly IPropertyValueExpressionProcessor propertyValueExpressionProcessor;
        private readonly IExpressionProcessor expressionProcessor;

        public PropertyAssignmentProcessor(IPropertyValueExpressionProcessor propertyValueExpressionProcessor, IExpressionProcessor expressionProcessor)
        {
            this.propertyValueExpressionProcessor = propertyValueExpressionProcessor;
            this.expressionProcessor = expressionProcessor;
        }

        public async Task<IPropertyAssignment> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IPropertyAssignmentParameterSet parameters) =>
            await new PropertyAssignment().AggregateBuildAsync(
                new IInnerProcess<IPropertyAssignment>[]
                {
                    new InnerProcess<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        this.propertyValueExpressionProcessor,
                        (g) => PropertyAssignmentProcessor.CreatePropertyValueExpressionParameterSet(parameters),
                        (g, r) => r.PropertyValueExpression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    ),
                    new InnerProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssignment>(
                        this.expressionProcessor,
                        (g) => PropertyAssignmentProcessor.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options
            );

        private static IPropertyValueExpressionParameterSet CreatePropertyValueExpressionParameterSet(IPropertyAssignmentParameterSet parameters) =>
          new PropertyValueExpressionParameterSet(
              parameters.Value,
              parameters.Class,
              parameters.ValueMatchBy
          );

        private static ExpressionParameterSet CreateExpressionParameterSet(PrimitiveSet primitives, IPropertyAssignmentParameterSet parameters, Neuron neuron) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        parameters.Property,
                        primitives.Unit
                    ),
                    new UnitParameterSet(
                        neuron,
                        primitives.NominalModifier
                    )
                }
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, IPropertyAssignmentParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet>(
                    this.propertyValueExpressionProcessor,
                    (n) => PropertyAssignmentProcessor.CreatePropertyValueExpressionParameterSet(parameters)
                ),
                new GrannyQueryInner<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    this.expressionProcessor,
                    (n) => PropertyAssignmentProcessor.CreateExpressionParameterSet(options.Primitives, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IPropertyAssignmentParameterSet parameters, out IPropertyAssignment result) =>
             new PropertyAssignment().AggregateTryParse(
                new IInnerProcess<IPropertyAssignment>[]
                {
                    new InnerProcess<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        this.propertyValueExpressionProcessor,
                        (g) => PropertyAssignmentProcessor.CreatePropertyValueExpressionParameterSet(parameters),
                        (g, r) => r.PropertyValueExpression = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssignment>(
                        this.expressionProcessor,
                        (g) => PropertyAssignmentProcessor.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                out result
            );
    }
}
