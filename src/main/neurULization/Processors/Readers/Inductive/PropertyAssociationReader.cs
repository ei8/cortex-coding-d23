using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyAssociationReader : IPropertyAssociationReader
    {
        private readonly IPropertyAssignmentReader propertyAssignmentReader;
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;
        private readonly IAggregateParser aggregateParser;

        public PropertyAssociationReader(
            IPropertyAssignmentReader propertyAssignmentReader, 
            IExpressionReader expressionReader, 
            IPrimitiveSet primitives,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(propertyAssignmentReader, nameof(propertyAssignmentReader));
            AssertionConcern.AssertArgumentNotNull(expressionReader, nameof(expressionReader));
            AssertionConcern.AssertArgumentNotNull(primitives, nameof(primitives));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.propertyAssignmentReader = propertyAssignmentReader;
            this.expressionReader = expressionReader;
            this.primitives = primitives;
            this.aggregateParser = aggregateParser;
        }

        private static IGreatGrannyInfoSuperset<IPropertyAssociation> CreateGreatGrannies(
            IPropertyAssignmentReader propertyAssignmentReader,
            IExpressionReader expressionReader,
            IPropertyAssociationParameterSet parameters,
            Ensemble ensemble,
            IPrimitiveSet primitives
        ) =>
            GreatGrannyInfoSuperset<IPropertyAssociation>.Create(
                new GreatGrannyInfoSet<IPropertyAssociation>[] {
                    ProcessHelper.CreateGreatGrannyCandidateSet(
                        ensemble,
                        parameters.Granny,
                        gc => new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssociation>(
                            gc,
                            expressionReader,
                            () => PropertyAssociationReader.CreateExpressionParameterSet(primitives, parameters, gc),
                            (g, r) => r.Expression = g
                        )
                    ),
                    new GreatGrannyInfoSet<IPropertyAssociation>(
                        new IGreatGrannyInfo<IPropertyAssociation>[] {
                            new DependentGreatGrannyInfo<IPropertyAssignment, IPropertyAssignmentReader, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                                propertyAssignmentReader,
                                g => PropertyAssociationReader.CreatePropertyAssignmentParameterSet(
                                    parameters,
                                    ((IExpression) g).Units.GetValueUnitGranniesByTypeId(primitives.DirectObject.Id).Single().Value
                                ),
                                (g, r) => r.PropertyAssignment = g
                            )
                        }
                    )
                }
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IPrimitiveSet primitives,
            IPropertyAssociationParameterSet parameters,
            Neuron unitGranny
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        primitives.Has,
                        primitives.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        primitives.DirectObject
                    )
                }
            );

        private static IPropertyAssignmentParameterSet CreatePropertyAssignmentParameterSet(IPropertyAssociationParameterSet parameters, Neuron value) =>
            new PropertyAssignmentParameterSet(
                value,
                parameters.Property,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IPropertyAssociationParameterSet parameters, out IPropertyAssociation result) =>
            this.aggregateParser.TryParse<PropertyAssociation, IPropertyAssociation>(
                parameters.Granny,
                PropertyAssociationReader.CreateGreatGrannies(
                    this.propertyAssignmentReader,
                    this.expressionReader,
                    parameters,
                    ensemble,
                    this.primitives
                ),
                new IGreatGrannyProcess<IPropertyAssociation>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssociation>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IPropertyAssignment, IPropertyAssignmentReader, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                out result
            );
    }
}
