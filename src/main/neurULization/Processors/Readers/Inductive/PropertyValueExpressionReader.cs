using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyValueExpressionReader : IPropertyValueExpressionReader
    {
        private readonly IValueExpressionReader valueExpressionReader;
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;
        private readonly IAggregateParser aggregateParser;
        private static readonly IGreatGrannyProcess<IPropertyValueExpression>[] targets = new IGreatGrannyProcess<IPropertyValueExpression>[]
            {
                new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyValueExpression>(
                    ProcessHelper.TryParse
                    ),
                new GreatGrannyProcess<IValueExpression, IValueExpressionReader, IValueExpressionParameterSet, IPropertyValueExpression>(
                    ProcessHelper.TryParse
                    )
            };

    public PropertyValueExpressionReader(
            IValueExpressionReader valueExpressionReader, 
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(valueExpressionReader, nameof(valueExpressionReader));
            AssertionConcern.AssertArgumentNotNull(expressionReader, nameof(expressionReader));
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.valueExpressionReader = valueExpressionReader;
            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
            this.aggregateParser = aggregateParser;
        }

        private static IGreatGrannyInfoSuperset<IPropertyValueExpression> CreateGreatGrannies(
            IValueExpressionReader valueExpressionReader,
            IExpressionReader expressionReader,
            IPropertyValueExpressionParameterSet parameters,
            Network network,
            IExternalReferenceSet externalReferences
        ) =>
            GreatGrannyInfoSuperset<IPropertyValueExpression>.Create(
                new GreatGrannyInfoSet<IPropertyValueExpression>[] {
                    ProcessHelper.CreateGreatGrannyCandidateSet(
                        network,
                        parameters.Granny,
                        gc => new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyValueExpression>(
                            gc,
                            expressionReader,
                            () => PropertyValueExpressionReader.CreateExpressionParameterSet(externalReferences, parameters, gc),
                            (g, r) => r.Expression = g
                        ),
                        targets[0]
                    ),
                    new GreatGrannyInfoSet<IPropertyValueExpression>(
                        new IGreatGrannyInfo<IPropertyValueExpression>[]
                        {
                            new DependentGreatGrannyInfo<IValueExpression, IValueExpressionReader, IValueExpressionParameterSet, IPropertyValueExpression>(
                                valueExpressionReader,
                                g => PropertyValueExpressionReader.CreateValueExpressionParameterSet(
                                    parameters,
                                    ((IExpression) g).Units.GetValueUnitGranniesByTypeId(externalReferences.Unit.Id).Single().Value
                                    ),
                                (g, r) => r.ValueExpression = g
                            )
                        },
                        targets[1]
                    )
                }
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyValueExpressionParameterSet parameters,
            Neuron unitGranny
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        externalReferences.Unit
                    ),
                    UnitParameterSet.CreateWithValueAndType(
                        externalReferences.Of,
                        externalReferences.Case
                    )
                }
            );

        private static IValueExpressionParameterSet CreateValueExpressionParameterSet(IPropertyValueExpressionParameterSet parameters, Neuron value) =>
            new ValueExpressionParameterSet(
                value,
                parameters.Class
            );

        public bool TryParse(Network network, IPropertyValueExpressionParameterSet parameters, out IPropertyValueExpression result) =>
            this.aggregateParser.TryParse<PropertyValueExpression, IPropertyValueExpression>(
                parameters.Granny,
                PropertyValueExpressionReader.CreateGreatGrannies(
                    this.valueExpressionReader,
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
