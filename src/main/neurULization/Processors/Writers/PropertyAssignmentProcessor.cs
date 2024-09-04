using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyAssignmentProcessor : IPropertyAssignmentProcessor
    {
        private readonly IPropertyValueExpressionProcessor propertyValueExpressionProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly Readers.Deductive.IPropertyAssignmentProcessor readProcessor;

        public PropertyAssignmentProcessor(
            IPropertyValueExpressionProcessor propertyValueExpressionProcessor, 
            IExpressionProcessor expressionProcessor,
            Readers.Deductive.IPropertyAssignmentProcessor readProcessor
            )
        {
            this.propertyValueExpressionProcessor = propertyValueExpressionProcessor;
            this.expressionProcessor = expressionProcessor;
            this.readProcessor = readProcessor;
        }

        public async Task<IPropertyAssignment> BuildAsync(Ensemble ensemble, Id23neurULizerWriteOptions options, IPropertyAssignmentParameterSet parameters) =>
            await new PropertyAssignment().AggregateBuildAsync(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcessAsync<IPropertyAssignment>[]
                {
                    new GreatGrannyProcessAsync<IPropertyValueExpression, IPropertyValueExpressionProcessor, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                         ProcessHelper.ObtainWithAggregateParamsAsync
                    ),
                    new GreatGrannyProcessAsync<IExpression, IExpressionProcessor, IExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options
            );

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

        public IGrannyReadProcessor<IPropertyAssignment, IPropertyAssignmentParameterSet> ReadProcessor => this.readProcessor;
    }
}
