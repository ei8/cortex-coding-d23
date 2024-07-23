using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyAssociationProcessor : IPropertyAssociationProcessor
    {
        private readonly IPropertyAssignmentProcessor propertyAssignmentProcessor;
        private readonly IExpressionProcessor expressionProcessor;

        public PropertyAssociationProcessor(IPropertyAssignmentProcessor propertyAssignmentProcessor, IExpressionProcessor expressionProcessor)
        {
            this.propertyAssignmentProcessor = propertyAssignmentProcessor;
            this.expressionProcessor = expressionProcessor;
        }

        public async Task<IPropertyAssociation> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IPropertyAssociationParameterSet parameters) =>
            await new PropertyAssociation().AggregateBuildAsync(
                this.CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcessAsync<IPropertyAssociation>[]
                {
                    new GreatGrannyProcessAsync<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    ),
                    new GreatGrannyProcessAsync<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssociation>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options
            );

        private IEnumerable<IGreatGrannyInfo<IPropertyAssociation>> CreateGreatGrannies(Id23neurULizerOptions options, IPropertyAssociationParameterSet parameters) =>
          new IGreatGrannyInfo<IPropertyAssociation>[]
          {
              new GreatGrannyInfo<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                    this.propertyAssignmentProcessor,
                    (g) => PropertyAssociationProcessor.CreatePropertyAssignmentParameterSet(parameters),
                    (g, r) => r.PropertyAssignment = g
                ),
                new GreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssociation>(
                    this.expressionProcessor,
                    (g) => PropertyAssociationProcessor.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
          };

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
                new GrannyQueryInner<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet>(
                    this.propertyAssignmentProcessor,
                    (n) => PropertyAssociationProcessor.CreatePropertyAssignmentParameterSet(parameters)
                ),
                new GrannyQueryInner<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    this.expressionProcessor,
                    (n) => PropertyAssociationProcessor.CreateExpressionParameterSet(options.Primitives, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IPropertyAssociationParameterSet parameters, out IPropertyAssociation result) =>
            new PropertyAssociation().AggregateTryParse(
                this.CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcess<IPropertyAssociation>[]
                {
                    new GreatGrannyProcess<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssociation>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                out result
            );
    }
}
