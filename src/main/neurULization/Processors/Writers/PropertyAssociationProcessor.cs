using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyAssociationProcessor : IPropertyAssociationProcessor
    {
        private readonly IPropertyAssignmentProcessor propertyAssignmentProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly Readers.Deductive.IPropertyAssociationProcessor readProcessor;

        public PropertyAssociationProcessor(
            IPropertyAssignmentProcessor propertyAssignmentProcessor, 
            IExpressionProcessor expressionProcessor,
            Readers.Deductive.IPropertyAssociationProcessor readProcessor
            )
        {
            this.propertyAssignmentProcessor = propertyAssignmentProcessor;
            this.expressionProcessor = expressionProcessor;
            this.readProcessor = readProcessor;
        }

        public async Task<IPropertyAssociation> BuildAsync(Ensemble ensemble, Id23neurULizerWriteOptions options, IPropertyAssociationParameterSet parameters) =>
            await new PropertyAssociation().AggregateBuildAsync(
                CreateGreatGrannies(options, parameters),
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

        private IEnumerable<IGreatGrannyInfo<IPropertyAssociation>> CreateGreatGrannies(Id23neurULizerWriteOptions options, IPropertyAssociationParameterSet parameters) =>
          new IGreatGrannyInfo<IPropertyAssociation>[]
          {
              new IndependentGreatGrannyInfo<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                    propertyAssignmentProcessor,
                    () => CreatePropertyAssignmentParameterSet(parameters),
                    (g, r) => r.PropertyAssignment = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssociation>(
                    expressionProcessor,
                    (g) => CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
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

        public IGrannyReadProcessor<IPropertyAssociation, IPropertyAssociationParameterSet> ReadProcessor => this.readProcessor;
    }
}
