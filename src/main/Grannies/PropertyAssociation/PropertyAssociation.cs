using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyAssociation : IPropertyAssociation
    {
        private readonly IPropertyAssignment propertyAssignmentProcessor;
        private readonly IExpression expressionProcessor;

        public PropertyAssociation(IPropertyAssignment propertyAssignmentProcessor, IExpression expressionProcessor)
        {
            this.propertyAssignmentProcessor = propertyAssignmentProcessor;
            this.expressionProcessor = expressionProcessor;
        }

        public async Task<IPropertyAssociation> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IPropertyAssociationParameterSet parameters) =>
            await new PropertyAssociation(this.propertyAssignmentProcessor, this.expressionProcessor).AggregateBuildAsync(
                new IInnerProcess<IPropertyAssociation>[]
                {
                    new InnerProcess<IPropertyAssignment, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        this.propertyAssignmentProcessor,
                        (g) => PropertyAssociation.CreatePropertyAssignmentParameterSet(parameters),
                        (g, r) => r.PropertyAssignment = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    ),
                    new InnerProcess<IExpression, IExpressionParameterSet, IPropertyAssociation>(
                        this.expressionProcessor,
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

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, IPropertyAssociationParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<IPropertyAssignment, IPropertyAssignmentParameterSet>(
                    this.propertyAssignmentProcessor,
                    (n) => PropertyAssociation.CreatePropertyAssignmentParameterSet(parameters)
                ),
                new GrannyQueryInner<IExpression, IExpressionParameterSet>(
                    this.expressionProcessor,
                    (n) => PropertyAssociation.CreateExpressionParameterSet(options.Primitives, parameters, n)
                )
            };

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IPropertyAssociationParameterSet parameters, out IPropertyAssociation result) =>
            (result = new PropertyAssociation(this.propertyAssignmentProcessor, this.expressionProcessor).AggregateTryParse(
                new IInnerProcess<IPropertyAssociation>[]
                {
                    new InnerProcess<IPropertyAssignment, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        this.propertyAssignmentProcessor,
                        (g) => PropertyAssociation.CreatePropertyAssignmentParameterSet(parameters),
                        (g, r) => r.PropertyAssignment = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<IExpression, IExpressionParameterSet, IPropertyAssociation>(
                        this.expressionProcessor,
                        (g) => PropertyAssociation.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            )) != null;

        public IPropertyAssignment PropertyAssignment { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }
    }
}
