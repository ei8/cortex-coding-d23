using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyAssociationProcessor : IPropertyAssociationProcessor
    {
        private readonly IPropertyAssignmentProcessor propertyAssignmentProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly IPrimitiveSet primitives;

        public PropertyAssociationProcessor(IPropertyAssignmentProcessor propertyAssignmentProcessor, IExpressionProcessor expressionProcessor, IPrimitiveSet primitives)
        {
            this.propertyAssignmentProcessor = propertyAssignmentProcessor;
            this.expressionProcessor = expressionProcessor;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssociation>> CreateGreatGrannies(
            IPropertyAssignmentProcessor propertyAssignmentProcessor,
            IExpressionProcessor expressionProcessor,
            IPropertyAssociationParameterSet parameters,
            Ensemble ensemble,
            IPrimitiveSet primitives
        )
        {
            var ppid = parameters.Property.Tag.ToString();
            return ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssociation>(
                    expressionProcessor,
                    () => PropertyAssociationProcessor.CreateExpressionParameterSet(primitives, parameters, gc),
                    (g, r) => r.Expression = g
                )
            ).Concat(
                new IGreatGrannyInfo<IPropertyAssociation>[]
                {
                    new DependentGreatGrannyInfo<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        propertyAssignmentProcessor,
                        g => CreatePropertyAssignmentParameterSet(
                            parameters,
                            ((IExpression) g).Units.GetValueUnitGranniesByTypeId(primitives.DirectObject.Id).Single().Value
                        ),
                        (g, r) => r.PropertyAssignment = g
                    )
                }
            );
        }

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IPrimitiveSet primitives,
            IPropertyAssociationParameterSet parameters,
            Neuron unitGranny
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        primitives.Has,
                        primitives.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        primitives.DirectObject
                    )
                }
            );

        private static IPropertyAssignmentParameterSet CreatePropertyAssignmentParameterSet(IPropertyAssociationParameterSet parameters, Neuron value) =>
            new PropertyAssignmentParameterSet(
                value,
                parameters.Property,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IPropertyAssociationParameterSet parameters, out IPropertyAssociation result) =>
            new PropertyAssociation().AggregateTryParse(
                parameters.Granny,
                PropertyAssociationProcessor.CreateGreatGrannies(
                    propertyAssignmentProcessor,
                    expressionProcessor,
                    parameters,
                    ensemble,
                    this.primitives
                ),
                new IGreatGrannyProcess<IPropertyAssociation>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssociation>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                2,
                out result
            );
    }
}
