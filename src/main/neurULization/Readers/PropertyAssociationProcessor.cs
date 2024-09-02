using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
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

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssociation>> CreateGreatGrannies(
            IPropertyAssignmentProcessor propertyAssignmentProcessor,
            IExpressionProcessor expressionProcessor,
            Id23neurULizerReadOptions options,
            IPropertyAssociationParameterSet parameters,
            Ensemble ensemble
        )
        {
            var ppid = parameters.Property.Tag.ToString();
            return ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssociation>(
                    expressionProcessor,
                    () => PropertyAssociationProcessor.CreateExpressionParameterSet(options.Primitives, parameters, gc),
                    (g, r) => r.Expression = g
                )
            ).Concat(
                new IGreatGrannyInfo<IPropertyAssociation>[]
                {
                    new DependentGreatGrannyInfo<IPropertyAssignment, IPropertyAssignmentProcessor, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        propertyAssignmentProcessor,
                        g => PropertyAssociationProcessor.CreatePropertyAssignmentParameterSet(
                            parameters,
                            ((IExpression) g).Units.GetValueUnitGranniesByTypeId(options.Primitives.DirectObject.Id).Single().Value
                        ),
                        (g, r) => r.PropertyAssignment = g
                    )
                }
            );
        }

        private static ExpressionParameterSet CreateExpressionParameterSet(
            PrimitiveSet primitives, 
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

        public bool TryParse(Ensemble ensemble, Id23neurULizerReadOptions options, IPropertyAssociationParameterSet parameters, out IPropertyAssociation result) =>
            new PropertyAssociation().AggregateTryParse(
                parameters.Granny,
                PropertyAssociationProcessor.CreateGreatGrannies(
                    this.propertyAssignmentProcessor,
                    this.expressionProcessor,
                    options, 
                    parameters,
                    ensemble
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
                options,
                2,
                out result
            );
    }
}
