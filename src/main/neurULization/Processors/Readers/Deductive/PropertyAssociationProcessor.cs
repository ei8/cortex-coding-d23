using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyAssociationProcessor : IPropertyAssociationProcessor
    {
        private readonly IPropertyAssignmentProcessor propertyAssignmentProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly IPrimitiveSet primitives;

        public PropertyAssociationProcessor(
            IPropertyAssignmentProcessor propertyAssignmentProcessor, 
            IExpressionProcessor expressionProcessor, 
            IPrimitiveSet primitives
        )
        {
            this.propertyAssignmentProcessor = propertyAssignmentProcessor;
            this.expressionProcessor = expressionProcessor;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssociation>> CreateGreatGrannies(
            IPropertyAssignmentProcessor propertyAssignmentProcessor,
            IExpressionProcessor expressionProcessor,
            IPropertyAssociationParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
          new IGreatGrannyInfo<IPropertyAssociation>[]
          {
              new IndependentGreatGrannyInfo<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                    propertyAssignmentProcessor,
                    () => PropertyAssociationProcessor.CreatePropertyAssignmentParameterSet(parameters),
                    (g, r) => r.PropertyAssignment = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssociation>(
                    expressionProcessor,
                    (g) => PropertyAssociationProcessor.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
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

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IPropertyAssociationParameterSet parameters, Neuron neuron) =>
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

        public IEnumerable<IGrannyQuery> GetQueries(IPropertyAssociationParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet>(
                    propertyAssignmentProcessor,
                    (n) => CreatePropertyAssignmentParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    expressionProcessor,
                    (n) => CreateExpressionParameterSet(this.primitives, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Ensemble ensemble, IPropertyAssociationParameterSet parameters, out IPropertyAssociation result) =>
            new PropertyAssociation().AggregateTryParse(
                PropertyAssociationProcessor.CreateGreatGrannies(
                    this.propertyAssignmentProcessor,
                    this.expressionProcessor,
                    parameters,
                    this.primitives
                ),
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
                out result
            );
    }
}
