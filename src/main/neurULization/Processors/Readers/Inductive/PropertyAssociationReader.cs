﻿using ei8.Cortex.Coding.d23.Grannies;
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
        private static readonly IGreatGrannyProcess<IPropertyAssociation>[] targets = new IGreatGrannyProcess<IPropertyAssociation>[]
            {
                new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssociation>(
                    ProcessHelper.TryParse
                ),
                new GreatGrannyProcess<IPropertyAssignment, IPropertyAssignmentReader, IPropertyAssignmentParameterSet, IPropertyAssociation>(
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

        private static IGreatGrannyInfoSuperset<IPropertyAssociation> CreateGreatGrannies(
            IPropertyAssignmentReader propertyAssignmentReader,
            IExpressionReader expressionReader,
            IPropertyAssociationParameterSet parameters,
            Network network,
            IExternalReferenceSet externalReferences
        ) =>
            GreatGrannyInfoSuperset<IPropertyAssociation>.Create(
                new GreatGrannyInfoSet<IPropertyAssociation>[] {
                    ProcessHelper.CreateGreatGrannyCandidateSet(
                        network,
                        parameters.Granny,
                        gc => new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssociation>(
                            gc,
                            expressionReader,
                            () => PropertyAssociationReader.CreateExpressionParameterSet(externalReferences, parameters, gc),
                            (g, r) => r.Expression = g
                        ),
                        targets[0]
                    ),
                    new GreatGrannyInfoSet<IPropertyAssociation>(
                        new IGreatGrannyInfo<IPropertyAssociation>[] {
                            new DependentGreatGrannyInfo<IPropertyAssignment, IPropertyAssignmentReader, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                                propertyAssignmentReader,
                                g => PropertyAssociationReader.CreatePropertyAssignmentParameterSet(
                                    parameters,
                                    ((IExpression) g).Units.GetValueUnitGranniesByTypeId(externalReferences.DirectObject.Id).Single().Value
                                ),
                                (g, r) => r.PropertyAssignment = g
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

        public bool TryParse(Network network, IPropertyAssociationParameterSet parameters, out IPropertyAssociation result) =>
            this.aggregateParser.TryParse<PropertyAssociation, IPropertyAssociation>(
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
