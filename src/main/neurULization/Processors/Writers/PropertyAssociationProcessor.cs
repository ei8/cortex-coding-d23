using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyAssociationProcessor : IPropertyAssociationProcessor
    {
        private readonly IPropertyAssignmentProcessor propertyAssignmentProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly Readers.Deductive.IPropertyAssociationProcessor readProcessor;
        private readonly IPrimitiveSet primitives;

        public PropertyAssociationProcessor(
            IPropertyAssignmentProcessor propertyAssignmentProcessor, 
            IExpressionProcessor expressionProcessor,
            Readers.Deductive.IPropertyAssociationProcessor readProcessor,
            IPrimitiveSet primitives
        )
        {
            this.propertyAssignmentProcessor = propertyAssignmentProcessor;
            this.expressionProcessor = expressionProcessor;
            this.readProcessor = readProcessor;
            this.primitives = primitives;
        }

        public IPropertyAssociation Build(Ensemble ensemble, IPropertyAssociationParameterSet parameters) =>
            new PropertyAssociation().AggregateBuild(
                PropertyAssociationProcessor.CreateGreatGrannies(
                    this.propertyAssignmentProcessor,
                    this.expressionProcessor,
                    parameters,
                    this.primitives
                ),
                new IGreatGrannyProcess<IPropertyAssociation>[]
                {
                    new GreatGrannyProcess<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        ProcessHelper.ParseBuild
                    ),
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssociation>(
                        ProcessHelper.ParseBuild
                    )
                },
                ensemble
            );

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

        public IGrannyReadProcessor<IPropertyAssociation, IPropertyAssociationParameterSet> ReadProcessor => this.readProcessor;
    }
}
