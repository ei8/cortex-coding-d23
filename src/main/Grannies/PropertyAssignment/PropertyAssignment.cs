using ei8.Cortex.Coding.d23.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyAssignment : IPropertyAssignment
    {
        public async Task<IPropertyAssignment> BuildAsync(Ensemble ensemble, IneurULizationOptions options, IPropertyAssignmentParameterSet parameters) =>
            await new PropertyAssignment().AggregateBuildAsync(
                new IInnerProcess<PropertyAssignment>[]
                {
                    new InnerProcess<PropertyValueExpression, IPropertyValueExpression, IPropertyValueExpressionParameterSet, PropertyAssignment>(
                        (g) => PropertyAssignment.CreatePropertyValueExpressionParameterSet(parameters),
                        (g, r) => r.PropertyValueExpression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, PropertyAssignment>(
                        (g) => PropertyAssignment.CreateExpressionParameterSet(options.ToInternal().Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options.ToInternal(),
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

        public IEnumerable<IGrannyQuery> GetQueries(IneurULizationOptions options, IPropertyAssignmentParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<PropertyValueExpression, IPropertyValueExpression, IPropertyValueExpressionParameterSet>(
                    (n) => PropertyAssignment.CreatePropertyValueExpressionParameterSet(parameters)
                ),
                new GrannyQueryInner<Expression, IExpression, IExpressionParameterSet>(
                    (n) => PropertyAssignment.CreateExpressionParameterSet(options.ToInternal().Primitives, parameters, n)
                )
            };

        public bool TryParse(Ensemble ensemble, IneurULizationOptions options, IPropertyAssignmentParameterSet parameters, out IPropertyAssignment result) =>
            (result = new PropertyAssignment().AggregateTryParse(
                new IInnerProcess<PropertyAssignment>[]
                {
                    new InnerProcess<PropertyValueExpression, IPropertyValueExpression, IPropertyValueExpressionParameterSet, PropertyAssignment>(
                        (g) => PropertyAssignment.CreatePropertyValueExpressionParameterSet(parameters),
                        (g, r) => r.PropertyValueExpression = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, PropertyAssignment>(
                        (g) => PropertyAssignment.CreateExpressionParameterSet(options.ToInternal().Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options.ToInternal(),
                (n, r) => r.Neuron = n
            )) != null;

        public IPropertyValueExpression PropertyValueExpression { get; private set; }

        public IExpression Expression { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
