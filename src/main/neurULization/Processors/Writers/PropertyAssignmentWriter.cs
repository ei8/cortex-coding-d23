using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyAssignmentWriter : IPropertyAssignmentWriter
    {
        private readonly IPropertyValueExpressionWriter propertyValueExpressionWriter;
        private readonly IExpressionWriter expressionWriter;
        private readonly Readers.Deductive.IPropertyAssignmentReader reader;
        private readonly IExternalReferenceSet externalReferences;

        public PropertyAssignmentWriter(
            IPropertyValueExpressionWriter propertyValueExpressionWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyAssignmentReader reader,
            IExternalReferenceSet externalReferences
            )
        {
            this.propertyValueExpressionWriter = propertyValueExpressionWriter;
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.externalReferences = externalReferences;
        }

        public IPropertyAssignment Build(Network network, IPropertyAssignmentParameterSet parameters) =>
            new PropertyAssignment().AggregateBuild(
                PropertyAssignmentWriter.CreateGreatGrannies(
                    this.propertyValueExpressionWriter,
                    this.expressionWriter,
                    parameters,
                    externalReferences
                ),
                new IGreatGrannyProcess<IPropertyAssignment>[]
                {
                    new GreatGrannyProcess<IPropertyValueExpression, IPropertyValueExpressionWriter, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                         ProcessHelper.ParseBuild
                    ),
                    new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.ParseBuild
                    )
                },
                network
            );

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssignment>> CreateGreatGrannies(
            IPropertyValueExpressionWriter propertyValueExpressionWriter,
            IExpressionWriter expressionWriter, 
            IPropertyAssignmentParameterSet parameters,
            IExternalReferenceSet externalReferences
        ) =>
            new IGreatGrannyInfo<IPropertyAssignment>[]
            {
                new IndependentGreatGrannyInfo<IPropertyValueExpression, IPropertyValueExpressionWriter, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                    propertyValueExpressionWriter,
                    () => PropertyAssignmentWriter.CreatePropertyValueExpressionParameterSet(parameters),
                    (g, r) => r.PropertyValueExpression = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyAssignment>(
                    expressionWriter,
                    (g) => PropertyAssignmentWriter.CreateExpressionParameterSet(externalReferences, parameters, g.Neuron),
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

        public IGrannyReader<IPropertyAssignment, IPropertyAssignmentParameterSet> Reader => this.reader;
    }
}
