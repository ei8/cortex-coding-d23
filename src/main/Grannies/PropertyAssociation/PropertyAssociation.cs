using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyAssociation : IPropertyAssociation
    {
        public async Task<IPropertyAssociation> BuildAsync(Ensemble ensemble, Id23neurULizationOptions options, IPropertyAssociationParameterSet parameters) =>
            await new PropertyAssociation().AggregateBuildAsync(
                new IInnerProcess<PropertyAssociation>[]
                {
                    new InnerProcess<PropertyAssignment, IPropertyAssignment, IPropertyAssignmentParameterSet, PropertyAssociation>(
                        (g) => PropertyAssociation.CreatePropertyAssignmentParameterSet(parameters),
                        (g, r) => r.PropertyAssignment = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, PropertyAssociation>(
                        (g) => PropertyAssociation.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            );

        private static IPropertyAssignmentParameterSet CreatePropertyAssignmentParameterSet(IPropertyAssociationParameterSet parameters) =>
            new PropertyAssignmentParameterSet(
                parameters.Property,
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(PrimitiveSet primitives, IPropertyAssociationParameterSet parameters, Neuron neuron) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        primitives.Has,
                        primitives.Unit
                    ),
                    new UnitParameterSet(
                        neuron,
                        primitives.DirectObject
                    )
                }
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizationOptions options, IPropertyAssociationParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<PropertyAssignment, IPropertyAssignment, IPropertyAssignmentParameterSet>(
                    (n) => PropertyAssociation.CreatePropertyAssignmentParameterSet(parameters)
                ),
                new GrannyQueryInner<Expression, IExpression, IExpressionParameterSet>(
                    (n) => PropertyAssociation.CreateExpressionParameterSet(options.Primitives, parameters, n)
                )
            };

        public bool TryParse(Ensemble ensemble, Id23neurULizationOptions options, IPropertyAssociationParameterSet parameters, out IPropertyAssociation result) =>
            (result = new PropertyAssociation().AggregateTryParse(
                new IInnerProcess<PropertyAssociation>[]
                {
                    new InnerProcess<PropertyAssignment, IPropertyAssignment, IPropertyAssignmentParameterSet, PropertyAssociation>(
                        (g) => PropertyAssociation.CreatePropertyAssignmentParameterSet(parameters),
                        (g, r) => r.PropertyAssignment = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, PropertyAssociation>(
                        (g) => PropertyAssociation.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            )) != null;

        public IPropertyAssignment PropertyAssignment { get; private set; }

        public IExpression Expression { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
