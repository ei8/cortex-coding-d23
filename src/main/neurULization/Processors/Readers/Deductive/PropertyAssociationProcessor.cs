using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
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

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerWriteOptions options, IPropertyAssociationParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet>(
                    propertyAssignmentProcessor,
                    (n) => CreatePropertyAssignmentParameterSet(parameters)
                ),
                new GrannyQueryInner<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    expressionProcessor,
                    (n) => CreateExpressionParameterSet(options.Primitives, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Ensemble ensemble, Id23neurULizerWriteOptions options, IPropertyAssociationParameterSet parameters, out IPropertyAssociation result) =>
            new PropertyAssociation().AggregateTryParse(
                CreateGreatGrannies(options, parameters),
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
