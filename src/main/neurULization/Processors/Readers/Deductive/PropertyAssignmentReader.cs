using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyAssignmentReader : IPropertyAssignmentReader
    {
        private readonly IPropertyValueExpressionReader propertyValueExpressionReader;
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;

        public PropertyAssignmentReader(
            IPropertyValueExpressionReader propertyValueExpressionReader, 
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences
        )
        {
            this.propertyValueExpressionReader = propertyValueExpressionReader;
            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssignment>> CreateGreatGrannies(
            IPropertyValueExpressionReader propertyValueExpressionReader,
            IExpressionReader expressionReader,
            IPropertyAssignmentParameterSet parameters,
            IExternalReferenceSet externalReferences
        ) =>
            new IGreatGrannyInfo<IPropertyAssignment>[]
            {
                new IndependentGreatGrannyInfo<IPropertyValueExpression, IPropertyValueExpressionReader, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                    propertyValueExpressionReader,
                    () => PropertyAssignmentReader.CreatePropertyValueExpressionParameterSet(parameters),
                    (g, r) => r.PropertyValueExpression = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssignment>(
                    expressionReader,
                    (g) => PropertyAssignmentReader.CreateExpressionParameterSet(externalReferences, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
            };

        private static IPropertyValueExpressionParameterSet CreatePropertyValueExpressionParameterSet(IPropertyAssignmentParameterSet parameters) =>
          new PropertyValueExpressionParameterSet(
              parameters.Value,
              parameters.Class,
              parameters.ValueMatchBy
          );

        private static ExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IPropertyAssignmentParameterSet parameters, Neuron neuron) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        parameters.Property,
                        externalReferences.Unit
                    ),
                    new UnitParameterSet(
                        neuron,
                        externalReferences.NominalModifier
                    )
                }
            );

        public IEnumerable<IGrannyQuery> GetQueries(Network network, IPropertyAssignmentParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IPropertyValueExpression, IPropertyValueExpressionReader, IPropertyValueExpressionParameterSet>(
                    this.propertyValueExpressionReader,
                    (n) => PropertyAssignmentReader.CreatePropertyValueExpressionParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionReader, IExpressionParameterSet>(
                    this.expressionReader,
                    (n) => PropertyAssignmentReader.CreateExpressionParameterSet(this.externalReferences, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Network network, IPropertyAssignmentParameterSet parameters, out IPropertyAssignment result) =>
            new PropertyAssignment().AggregateTryParse(
                PropertyAssignmentReader.CreateGreatGrannies(
                    this.propertyValueExpressionReader,
                    this.expressionReader,
                    parameters,
                    this.externalReferences
                ),
                new IGreatGrannyProcess<IPropertyAssignment>[]
                {
                    new GreatGrannyProcess<IPropertyValueExpression, IPropertyValueExpressionReader, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.TryParse
                        )
                },
                network,
                out result
            );
    }
}
