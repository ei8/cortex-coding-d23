using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyAssociationWriter : IPropertyAssociationWriter
    {
        private readonly IPropertyAssignmentWriter propertyAssignmentWriter;
        private readonly IExpressionWriter expressionWriter;
        private readonly Readers.Deductive.IPropertyAssociationReader reader;
        private readonly IExternalReferenceSet externalReferences;

        public PropertyAssociationWriter(
            IPropertyAssignmentWriter propertyAssignmentWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyAssociationReader reader,
            IExternalReferenceSet externalReferences
        )
        {
            this.propertyAssignmentWriter = propertyAssignmentWriter;
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.externalReferences = externalReferences;
        }

        public IPropertyAssociation Build(Network network, IPropertyAssociationParameterSet parameters) =>
            new PropertyAssociation().AggregateBuild(
                PropertyAssociationWriter.CreateGreatGrannies(
                    this.propertyAssignmentWriter,
                    this.expressionWriter,
                    parameters,
                    this.externalReferences
                ),
                new IGreatGrannyProcess<IPropertyAssociation>[]
                {
                    new GreatGrannyProcess<IPropertyAssignment, IPropertyAssignmentWriter, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        ProcessHelper.ParseBuild
                    ),
                    new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyAssociation>(
                        ProcessHelper.ParseBuild
                    )
                },
                network
            );

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssociation>> CreateGreatGrannies(
            IPropertyAssignmentWriter propertyAssignmentWriter,
            IExpressionWriter expressionWriter, 
            IPropertyAssociationParameterSet parameters,
            IExternalReferenceSet externalReferences
        ) =>
          new IGreatGrannyInfo<IPropertyAssociation>[]
          {
              new IndependentGreatGrannyInfo<IPropertyAssignment, IPropertyAssignmentWriter, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                    propertyAssignmentWriter,
                    () => PropertyAssociationWriter.CreatePropertyAssignmentParameterSet(parameters),
                    (g, r) => r.PropertyAssignment = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyAssociation>(
                    expressionWriter,
                    (g) => PropertyAssociationWriter.CreateExpressionParameterSet(externalReferences, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
          };

        private static IPropertyAssignmentParameterSet CreatePropertyAssignmentParameterSet(IPropertyAssociationParameterSet parameters) =>
            new PropertyAssignmentParameterSet(
                parameters.Property,
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IPropertyAssociationParameterSet parameters, Neuron neuron) =>
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

        public IGrannyReader<IPropertyAssociation, IPropertyAssociationParameterSet> Reader => this.reader;
    }
}
