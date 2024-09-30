using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyAssignmentReader : IPropertyAssignmentReader
    {
        private readonly IPropertyValueExpressionReader propertyValueExpressionReader;
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;

        public PropertyAssignmentReader(IPropertyValueExpressionReader propertyValueExpressionReader, IExpressionReader expressionReader, IPrimitiveSet primitives)
        {
            this.propertyValueExpressionReader = propertyValueExpressionReader;
            this.expressionReader = expressionReader;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssignment>> CreateGreatGrannies(
            IPropertyValueExpressionReader propertyValueExpressionReader,
            IExpressionReader expressionReader,
            IPropertyAssignmentParameterSet parameters,
            Ensemble ensemble,
            IPrimitiveSet primitives
        ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssignment>(
                    expressionReader,
                    () => PropertyAssignmentReader.CreateExpressionParameterSet(primitives, parameters, gc),
                    (g, r) => r.Expression = g
                )
            ).Concat(
                new IGreatGrannyInfo<IPropertyAssignment>[]
                {
                    new DependentGreatGrannyInfo<IPropertyValueExpression, IPropertyValueExpressionReader, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        propertyValueExpressionReader,
                        g => PropertyAssignmentReader.CreatePropertyValueExpressionParameterSet(
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
                PropertyAssignmentReader.CreateGreatGrannies(
                    this.propertyValueExpressionReader,
                    this.expressionReader,
                    parameters,
                    ensemble,
                    this.primitives
                    ),
                new IGreatGrannyProcess<IPropertyAssignment>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IPropertyValueExpression, IPropertyValueExpressionReader, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                2,
                out result
            );
    }
}
