using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyAssociationReader : IPropertyAssociationReader
    {
        private readonly IPropertyAssignmentReader propertyAssignmentReader;
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;
        private readonly IAggregateParser aggregateParser;
        private static readonly IGreatGrannyProcess<IPropertyValueAssociation>[] targets = new IGreatGrannyProcess<IPropertyValueAssociation>[]
            {
                new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyValueAssociation>(
                    ProcessHelper.TryParse
                ),
                new GreatGrannyProcess<IPropertyValueAssignment, IPropertyAssignmentReader, IPropertyAssignmentParameterSet, IPropertyValueAssociation>(
                    ProcessHelper.TryParse
                )
            };

    public PropertyAssociationReader(
            IPropertyAssignmentReader propertyAssignmentReader, 
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(propertyAssignmentReader, nameof(propertyAssignmentReader));
            AssertionConcern.AssertArgumentNotNull(expressionReader, nameof(expressionReader));
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.propertyAssignmentReader = propertyAssignmentReader;
            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
            this.aggregateParser = aggregateParser;
        }

        private static IGreatGrannyInfoSuperset<IPropertyValueAssociation> CreateGreatGrannies(
            IPropertyAssignmentReader propertyAssignmentReader,
            IExpressionReader expressionReader,
            IPropertyAssociationParameterSet parameters,
            Network network,
            IExternalReferenceSet externalReferences
        ) =>
            GreatGrannyInfoSuperset<IPropertyValueAssociation>.Create(
                new GreatGrannyInfoSet<IPropertyValueAssociation>[] {
                    ProcessHelper.CreateGreatGrannyCandidateSet(
                        network,
                        parameters.Granny,
                        gc => new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyValueAssociation>(
                            gc,
                            expressionReader,
                            () => PropertyAssociationReader.CreateExpressionParameterSet(externalReferences, parameters, gc),
                            (g, r) => r.Expression = g
                        ),
                        targets[0]
                    ),
                    new GreatGrannyInfoSet<IPropertyValueAssociation>(
                        new IGreatGrannyInfo<IPropertyValueAssociation>[] {
                            new DependentGreatGrannyInfo<IPropertyValueAssignment, IPropertyAssignmentReader, IPropertyAssignmentParameterSet, IPropertyValueAssociation>(
                                propertyAssignmentReader,
                                g => PropertyAssociationReader.CreatePropertyAssignmentParameterSet(
                                    parameters,
                                    ((IExpression) g).Units.GetValueUnitGranniesByTypeId(externalReferences.DirectObject.Id).Single().Value
                                ),
                                (g, r) => r.TypedGreatGranny = g
                            )
                        },
                        targets[1]
                    )
                }
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyAssociationParameterSet parameters,
            Neuron unitGranny
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        externalReferences.Has,
                        externalReferences.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        externalReferences.DirectObject
                    )
                }
            );

        private static IPropertyAssignmentParameterSet CreatePropertyAssignmentParameterSet(IPropertyAssociationParameterSet parameters, Neuron value) =>
            new PropertyAssignmentParameterSet(
                value,
                parameters.Property,
                parameters.Class
            );

        public bool TryParse(Network network, IPropertyAssociationParameterSet parameters, out IPropertyValueAssociation result) =>
            this.aggregateParser.TryParse<PropertyValueAssociation, IPropertyValueAssociation>(
                parameters.Granny,
                PropertyAssociationReader.CreateGreatGrannies(
                    this.propertyAssignmentReader,
                    this.expressionReader,
                    parameters,
                    network,
                    this.externalReferences
                ),
                network,
                out result
            );
    }
}
