using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyAssignment : IPropertyAssignment
    {
        private readonly IPropertyValueExpression propertyValueExpressionProcessor;
        private readonly IExpression expressionProcessor;

        public PropertyAssignment(IPropertyValueExpression propertyValueExpressionProcessor, IExpression expressionProcessor)
        {
            this.propertyValueExpressionProcessor = propertyValueExpressionProcessor;
            this.expressionProcessor = expressionProcessor;
        }

        public async Task<IPropertyAssignment> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IPropertyAssignmentParameterSet parameters) =>
            await new PropertyAssignment(this.propertyValueExpressionProcessor, this.expressionProcessor).AggregateBuildAsync(
                new IInnerProcess<IPropertyAssignment>[]
                {
                    new InnerProcess<IPropertyValueExpression, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        this.propertyValueExpressionProcessor,
                        (g) => PropertyAssignment.CreatePropertyValueExpressionParameterSet(parameters),
                        (g, r) => r.PropertyValueExpression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    ),
                    new InnerProcess<IExpression, IExpressionParameterSet, IPropertyAssignment>(
                        this.expressionProcessor,
                        (g) => PropertyAssignment.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
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
                new GrannyQueryInner<IPropertyValueExpression, IPropertyValueExpressionParameterSet>(
                    this.propertyValueExpressionProcessor,
                    (n) => PropertyAssignment.CreatePropertyValueExpressionParameterSet(parameters)
                ),
                new GrannyQueryInner<IExpression, IExpressionParameterSet>(
                    this.expressionProcessor,
                    (n) => PropertyAssignment.CreateExpressionParameterSet(options.Primitives, parameters, n)
                )
            };

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IPropertyAssignmentParameterSet parameters, out IPropertyAssignment result) =>
            (result = new PropertyAssignment(this.propertyValueExpressionProcessor, this.expressionProcessor).AggregateTryParse(
                new IInnerProcess<IPropertyAssignment>[]
                {
                    new InnerProcess<IPropertyValueExpression, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        this.propertyValueExpressionProcessor,
                        (g) => PropertyAssignment.CreatePropertyValueExpressionParameterSet(parameters),
                        (g, r) => r.PropertyValueExpression = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<IExpression, IExpressionParameterSet, IPropertyAssignment>(
                        this.expressionProcessor,
                        (g) => PropertyAssignment.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            )) != null;

        public IPropertyValueExpression PropertyValueExpression { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }
    }
}
