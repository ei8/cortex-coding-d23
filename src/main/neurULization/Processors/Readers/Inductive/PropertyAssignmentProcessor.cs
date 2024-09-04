using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyAssignmentProcessor : IPropertyAssignmentProcessor
    {
        private readonly IPropertyValueExpressionProcessor propertyValueExpressionProcessor;
        private readonly IExpressionProcessor expressionProcessor;

        public PropertyAssignmentProcessor(IPropertyValueExpressionProcessor propertyValueExpressionProcessor, IExpressionProcessor expressionProcessor)
        {
            this.propertyValueExpressionProcessor = propertyValueExpressionProcessor;
            this.expressionProcessor = expressionProcessor;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssignment>> CreateGreatGrannies(
            IPropertyValueExpressionProcessor propertyValueExpressionProcessor,
            IExpressionProcessor expressionProcessor,
            Id23neurULizerReadOptions options,
            IPropertyAssignmentParameterSet parameters,
            Ensemble ensemble
        ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssignment>(
                    expressionProcessor,
                    () => CreateExpressionParameterSet(options.Primitives, parameters, gc),
                    (g, r) => r.Expression = g
                )
            ).Concat(
                new IGreatGrannyInfo<IPropertyAssignment>[]
                {
                    new DependentGreatGrannyInfo<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        propertyValueExpressionProcessor,
                        g => CreatePropertyValueExpressionParameterSet(
                            parameters,
                            ((IExpression) g).Units.GetValueUnitGranniesByTypeId(options.Primitives.NominalModifier.Id).Single().Value
                            ),
                        (g, r) => r.PropertyValueExpression = g
                    )
                }
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            PrimitiveSet primitives,
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

        public bool TryParse(Ensemble ensemble, Id23neurULizerReadOptions options, IPropertyAssignmentParameterSet parameters, out IPropertyAssignment result) =>
            new PropertyAssignment().AggregateTryParse(
                parameters.Granny,
                CreateGreatGrannies(
                    propertyValueExpressionProcessor,
                    expressionProcessor,
                    options,
                    parameters,
                    ensemble
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
                options,
                2,
                out result
            );
    }
}
