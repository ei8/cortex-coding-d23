using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyAssignmentWriter : IPropertyAssignmentWriter
    {
        private readonly IPropertyValueExpressionWriter propertyValueExpressionWriter;
        private readonly IExpressionWriter expressionWriter;
        private readonly Readers.Deductive.IPropertyValueAssignmentReader reader;
        private readonly IExternalReferenceSet externalReferences;

        public PropertyAssignmentWriter(
            IPropertyValueExpressionWriter propertyValueExpressionWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyValueAssignmentReader reader,
            IExternalReferenceSet externalReferences
            )
        {
            this.propertyValueExpressionWriter = propertyValueExpressionWriter;
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.externalReferences = externalReferences;
        }

        public IPropertyValueAssignment Build(Network network, IPropertyValueAssignmentParameterSet parameters) =>
            new PropertyValueAssignment().AggregateBuild(
                PropertyAssignmentWriter.CreateGreatGrannies(
                    this.propertyValueExpressionWriter,
                    this.expressionWriter,
                    parameters,
                    externalReferences
                ),
                new IGreatGrannyProcess<IPropertyValueAssignment>[]
                {
                    new GreatGrannyProcess<IPropertyValueExpression, IPropertyValueExpressionWriter, IPropertyValueExpressionParameterSet, IPropertyValueAssignment>(
                         ProcessHelper.ParseBuild
                    ),
                    new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyValueAssignment>(
                        ProcessHelper.ParseBuild
                    )
                },
                network
            );

        private static IEnumerable<IGreatGrannyInfo<IPropertyValueAssignment>> CreateGreatGrannies(
            IPropertyValueExpressionWriter propertyValueExpressionWriter,
            IExpressionWriter expressionWriter, 
            IPropertyValueAssignmentParameterSet parameters,
            IExternalReferenceSet externalReferences
        ) =>
            new IGreatGrannyInfo<IPropertyValueAssignment>[]
            {
                new IndependentGreatGrannyInfo<IPropertyValueExpression, IPropertyValueExpressionWriter, IPropertyValueExpressionParameterSet, IPropertyValueAssignment>(
                    propertyValueExpressionWriter,
                    () => PropertyAssignmentWriter.CreatePropertyValueExpressionParameterSet(parameters),
                    (g, r) => r.PropertyValueExpression = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyValueAssignment>(
                    expressionWriter,
                    (g) => PropertyAssignmentWriter.CreateExpressionParameterSet(externalReferences, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
            };

        private static IPropertyValueExpressionParameterSet CreatePropertyValueExpressionParameterSet(IPropertyValueAssignmentParameterSet parameters) =>
          new PropertyValueExpressionParameterSet(
              parameters.Value,
              parameters.Class,
              parameters.ValueMatchBy
          );

        private static ExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IPropertyValueAssignmentParameterSet parameters, Neuron greatGranny) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        parameters.Property,
                        externalReferences.Unit
                    ),
                    new UnitParameterSet(
                        greatGranny,
                        externalReferences.NominalModifier
                    )
                }
            );

        public IGrannyReader<IPropertyValueAssignment, IPropertyValueAssignmentParameterSet> Reader => this.reader;
    }
}
