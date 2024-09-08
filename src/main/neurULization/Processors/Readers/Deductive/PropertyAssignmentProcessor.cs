using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
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

        private IEnumerable<IGreatGrannyInfo<IPropertyAssignment>> CreateGreatGrannies(Id23neurULizerWriteOptions options, IPropertyAssignmentParameterSet parameters) =>
            new IGreatGrannyInfo<IPropertyAssignment>[]
            {
                new IndependentGreatGrannyInfo<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                    propertyValueExpressionProcessor,
                    () => CreatePropertyValueExpressionParameterSet(parameters),
                    (g, r) => r.PropertyValueExpression = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssignment>(
                    expressionProcessor,
                    (g) => CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
            };

        private static IPropertyValueExpressionParameterSet CreatePropertyValueExpressionParameterSet(IPropertyAssignmentParameterSet parameters) =>
          new PropertyValueExpressionParameterSet(
              parameters.Value,
              parameters.Class,
              parameters.ValueMatchBy
          );

        private static ExpressionParameterSet CreateExpressionParameterSet(PrimitiveSet primitives, IPropertyAssignmentParameterSet parameters, Neuron neuron) =>
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

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerWriteOptions options, IPropertyAssignmentParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet>(
                    propertyValueExpressionProcessor,
                    (n) => CreatePropertyValueExpressionParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    expressionProcessor,
                    (n) => CreateExpressionParameterSet(options.Primitives, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Ensemble ensemble, Id23neurULizerWriteOptions options, IPropertyAssignmentParameterSet parameters, out IPropertyAssignment result) =>
            new PropertyAssignment().AggregateTryParse(
                CreateGreatGrannies(options, parameters),
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
                options,
                out result
            );
    }
}
