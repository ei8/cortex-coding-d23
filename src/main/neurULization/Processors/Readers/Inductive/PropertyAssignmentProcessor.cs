using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyAssignmentProcessor : IPropertyAssignmentProcessor
    {
        private readonly IPropertyValueExpressionProcessor propertyValueExpressionProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly IPrimitiveSet primitives;

        public PropertyAssignmentProcessor(IPropertyValueExpressionProcessor propertyValueExpressionProcessor, IExpressionProcessor expressionProcessor, IPrimitiveSet primitives)
        {
            this.propertyValueExpressionProcessor = propertyValueExpressionProcessor;
            this.expressionProcessor = expressionProcessor;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssignment>> CreateGreatGrannies(
            IPropertyValueExpressionProcessor propertyValueExpressionProcessor,
            IExpressionProcessor expressionProcessor,
            IPropertyAssignmentParameterSet parameters,
            Ensemble ensemble,
            IPrimitiveSet primitives
        ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssignment>(
                    expressionProcessor,
                    () => PropertyAssignmentProcessor.CreateExpressionParameterSet(primitives, parameters, gc),
                    (g, r) => r.Expression = g
                )
            ).Concat(
                new IGreatGrannyInfo<IPropertyAssignment>[]
                {
                    new DependentGreatGrannyInfo<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        propertyValueExpressionProcessor,
                        g => PropertyAssignmentProcessor.CreatePropertyValueExpressionParameterSet(
                            parameters,
                            ((IExpression) g).Units.GetValueUnitGranniesByTypeId(primitives.NominalModifier.Id).Single().Value
                            ),
                        (g, r) => r.PropertyValueExpression = g
                    )
                }
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IPrimitiveSet primitives,
            IPropertyAssignmentParameterSet parameters,
            Neuron unitGranny
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        parameters.Property,
                        primitives.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        primitives.NominalModifier
                    )
                }
            );

        private static IPropertyValueExpressionParameterSet CreatePropertyValueExpressionParameterSet(IPropertyAssignmentParameterSet parameters, Neuron value) =>
            new PropertyValueExpressionParameterSet(
                value,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, 
            IPropertyAssignmentParameterSet parameters, out IPropertyAssignment result) =>
            new PropertyAssignment().AggregateTryParse(
                parameters.Granny,
                PropertyAssignmentProcessor.CreateGreatGrannies(
                    propertyValueExpressionProcessor,
                    expressionProcessor,
                    parameters,
                    ensemble,
                    this.primitives
                    ),
                new IGreatGrannyProcess<IPropertyAssignment>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                2,
                out result
            );
    }
}
