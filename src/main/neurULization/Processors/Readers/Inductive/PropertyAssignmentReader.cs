using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyAssignmentReader : IPropertyAssignmentReader
    {
        private readonly IPropertyValueExpressionReader propertyValueExpressionReader;
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;
        private readonly IAggregateParser aggregateParser;
        private static readonly IGreatGrannyProcess<IPropertyAssignment>[] targets = 
            new IGreatGrannyProcess<IPropertyAssignment>[]
            {
                new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssignment>(
                    ProcessHelper.TryParse
                    ),
                new GreatGrannyProcess<IPropertyValueExpression, IPropertyValueExpressionReader, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                    ProcessHelper.TryParse
                    )
            };

        public PropertyAssignmentReader(
            IPropertyValueExpressionReader propertyValueExpressionReader, 
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(propertyValueExpressionReader, nameof(propertyValueExpressionReader));
            AssertionConcern.AssertArgumentNotNull(expressionReader, nameof(expressionReader));
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.propertyValueExpressionReader = propertyValueExpressionReader;
            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
            this.aggregateParser = aggregateParser;
        }

        private static IGreatGrannyInfoSuperset<IPropertyAssignment> CreateGreatGrannies(
            IPropertyValueExpressionReader propertyValueExpressionReader,
            IExpressionReader expressionReader,
            IPropertyAssignmentParameterSet parameters,
            Ensemble ensemble,
            IExternalReferenceSet externalReferences
        ) =>
            GreatGrannyInfoSuperset<IPropertyAssignment>.Create(
                new GreatGrannyInfoSet<IPropertyAssignment>[] {
                    ProcessHelper.CreateGreatGrannyCandidateSet(
                        ensemble,
                        parameters.Granny,
                        gc => new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssignment>(
                            gc,
                            expressionReader,
                            () => PropertyAssignmentReader.CreateExpressionParameterSet(externalReferences, parameters, gc),
                            (g, r) => r.Expression = g
                        ),
                        targets[0]
                    ),
                    new GreatGrannyInfoSet<IPropertyAssignment>(
                        new IGreatGrannyInfo<IPropertyAssignment>[]
                        {
                            new DependentGreatGrannyInfo<IPropertyValueExpression, IPropertyValueExpressionReader, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                                propertyValueExpressionReader,
                                g => PropertyAssignmentReader.CreatePropertyValueExpressionParameterSet(
                                    parameters,
                                    ((IExpression) g).Units.GetValueUnitGranniesByTypeId(externalReferences.NominalModifier.Id).Single().Value
                                    ),
                                (g, r) => r.PropertyValueExpression = g
                            )
                        },
                        targets[1]
                    )
                }
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyAssignmentParameterSet parameters,
            Neuron unitGranny
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        parameters.Property,
                        externalReferences.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        externalReferences.NominalModifier
                    )
                }
            );

        private static IPropertyValueExpressionParameterSet CreatePropertyValueExpressionParameterSet(IPropertyAssignmentParameterSet parameters, Neuron value) =>
            new PropertyValueExpressionParameterSet(
                value,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IPropertyAssignmentParameterSet parameters, out IPropertyAssignment result) =>
            this.aggregateParser.TryParse<PropertyAssignment, IPropertyAssignment>(
                parameters.Granny,
                PropertyAssignmentReader.CreateGreatGrannies(
                    this.propertyValueExpressionReader,
                    this.expressionReader,
                    parameters,
                    ensemble,
                    this.externalReferences
                ),
                ensemble,
                out result
            );
    }
}
