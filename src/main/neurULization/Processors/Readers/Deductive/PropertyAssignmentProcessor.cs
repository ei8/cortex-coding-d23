using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyAssignmentProcessor : IPropertyAssignmentProcessor
    {
        private readonly IPropertyValueExpressionProcessor propertyValueExpressionProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly IPrimitiveSet primitives;

        public PropertyAssignmentProcessor(
            IPropertyValueExpressionProcessor propertyValueExpressionProcessor, 
            IExpressionProcessor expressionProcessor, 
            IPrimitiveSet primitives
        )
        {
            this.propertyValueExpressionProcessor = propertyValueExpressionProcessor;
            this.expressionProcessor = expressionProcessor;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssignment>> CreateGreatGrannies(
            IPropertyValueExpressionProcessor propertyValueExpressionProcessor,
            IExpressionProcessor expressionProcessor,
            IPropertyAssignmentParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
            new IGreatGrannyInfo<IPropertyAssignment>[]
            {
                new IndependentGreatGrannyInfo<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                    propertyValueExpressionProcessor,
                    () => PropertyAssignmentProcessor.CreatePropertyValueExpressionParameterSet(parameters),
                    (g, r) => r.PropertyValueExpression = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssignment>(
                    expressionProcessor,
                    (g) => PropertyAssignmentProcessor.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
            };

        private static IPropertyValueExpressionParameterSet CreatePropertyValueExpressionParameterSet(IPropertyAssignmentParameterSet parameters) =>
          new PropertyValueExpressionParameterSet(
              parameters.Value,
              parameters.Class,
              parameters.ValueMatchBy
          );

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IPropertyAssignmentParameterSet parameters, Neuron neuron) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        parameters.Property,
                        primitives.Unit
                    ),
                    new UnitParameterSet(
                        neuron,
                        primitives.NominalModifier
                    )
                }
            );

        public IEnumerable<IGrannyQuery> GetQueries(IPropertyAssignmentParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet>(
                    propertyValueExpressionProcessor,
                    (n) => CreatePropertyValueExpressionParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    expressionProcessor,
                    (n) => CreateExpressionParameterSet(this.primitives, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Ensemble ensemble, IPropertyAssignmentParameterSet parameters, out IPropertyAssignment result) =>
            new PropertyAssignment().AggregateTryParse(
                PropertyAssignmentProcessor.CreateGreatGrannies(
                    this.propertyValueExpressionProcessor,
                    this.expressionProcessor,
                    parameters,
                    this.primitives
                ),
                new IGreatGrannyProcess<IPropertyAssignment>[]
                {
                    new GreatGrannyProcess<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                out result
            );
    }
}
