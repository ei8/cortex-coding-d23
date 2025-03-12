using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyAssociationWriter : IPropertyAssociationWriter
    {
        private readonly IPropertyAssignmentWriter propertyAssignmentWriter;
        private readonly IExpressionWriter expressionWriter;
        private readonly Readers.Deductive.IPropertyValueAssociationReader reader;
        private readonly IExternalReferenceSet externalReferences;

        public PropertyAssociationWriter(
            IPropertyAssignmentWriter propertyAssignmentWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyValueAssociationReader reader,
            IExternalReferenceSet externalReferences
        )
        {
            this.propertyAssignmentWriter = propertyAssignmentWriter;
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.externalReferences = externalReferences;
        }

        public IPropertyValueAssociation Build(Network network, IPropertyValueAssociationParameterSet parameters) =>
            new PropertyValueAssociation().AggregateBuild(
                PropertyAssociationWriter.CreateGreatGrannies(
                    this.propertyAssignmentWriter,
                    this.expressionWriter,
                    parameters,
                    this.externalReferences
                ),
                new IGreatGrannyProcess<IPropertyValueAssociation>[]
                {
                    new GreatGrannyProcess<IPropertyValueAssignment, IPropertyAssignmentWriter, IPropertyValueAssignmentParameterSet, IPropertyValueAssociation>(
                        ProcessHelper.ParseBuild
                    ),
                    new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyValueAssociation>(
                        ProcessHelper.ParseBuild
                    )
                },
                network
            );

        private static IEnumerable<IGreatGrannyInfo<IPropertyValueAssociation>> CreateGreatGrannies(
            IPropertyAssignmentWriter propertyAssignmentWriter,
            IExpressionWriter expressionWriter, 
            IPropertyValueAssociationParameterSet parameters,
            IExternalReferenceSet externalReferences
        ) =>
          new IGreatGrannyInfo<IPropertyValueAssociation>[]
          {
              new IndependentGreatGrannyInfo<IPropertyValueAssignment, IPropertyAssignmentWriter, IPropertyValueAssignmentParameterSet, IPropertyValueAssociation>(
                    propertyAssignmentWriter,
                    () => PropertyAssociationWriter.CreatePropertyAssignmentParameterSet(parameters),
                    (g, r) => r.PropertyAssignment = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyValueAssociation>(
                    expressionWriter,
                    (g) => PropertyAssociationWriter.CreateExpressionParameterSet(externalReferences, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
          };

        private static IPropertyValueAssignmentParameterSet CreatePropertyAssignmentParameterSet(IPropertyValueAssociationParameterSet parameters) =>
            new PropertyValueAssignmentParameterSet(
                parameters.Property,
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IPropertyValueAssociationParameterSet parameters, Neuron neuron) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        externalReferences.Has,
                        externalReferences.Unit
                    ),
                    new UnitParameterSet(
                        neuron,
                        externalReferences.DirectObject
                    )
                }
            );

        public IGrannyReader<IPropertyValueAssociation, IPropertyValueAssociationParameterSet> Reader => this.reader;
    }
}
