using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyValueExpressionReader : IPropertyValueExpressionReader
    {
        private readonly IValueExpressionReader valueExpressionReader;
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;
        private readonly IAggregateParser aggregateParser;

        public PropertyValueExpressionReader(
            IValueExpressionReader valueExpressionReader, 
            IExpressionReader expressionReader, 
            IPrimitiveSet primitives,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(valueExpressionReader, nameof(valueExpressionReader));
            AssertionConcern.AssertArgumentNotNull(expressionReader, nameof(expressionReader));
            AssertionConcern.AssertArgumentNotNull(primitives, nameof(primitives));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.valueExpressionReader = valueExpressionReader;
            this.expressionReader = expressionReader;
            this.primitives = primitives;
            this.aggregateParser = aggregateParser;
        }

        private static GreatGrannyInfoSuperset<IPropertyValueExpression> CreateGreatGrannies(
            IValueExpressionReader valueExpressionReader,
            IExpressionReader expressionReader,
            IPropertyValueExpressionParameterSet parameters,
            Ensemble ensemble,
            IPrimitiveSet primitives
        ) =>
            GreatGrannyInfoSuperset<IPropertyValueExpression>.Create(
                new GreatGrannyInfoSet<IPropertyValueExpression>[] {
                    ProcessHelper.CreateGreatGrannyCandidateSet(
                        ensemble,
                        parameters.Granny,
                        gc => new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyValueExpression>(
                            gc,
                            expressionReader,
                            () => PropertyValueExpressionReader.CreateExpressionParameterSet(primitives, parameters, gc),
                            (g, r) => r.Expression = g
                        )
                    ),
                    new GreatGrannyInfoSet<IPropertyValueExpression>(
                        new IGreatGrannyInfo<IPropertyValueExpression>[]
                        {
                            new DependentGreatGrannyInfo<IValueExpression, IValueExpressionReader, IValueExpressionParameterSet, IPropertyValueExpression>(
                                valueExpressionReader,
                                g => PropertyValueExpressionReader.CreateValueExpressionParameterSet(
                                    parameters,
                                    ((IExpression) g).Units.GetValueUnitGranniesByTypeId(primitives.Unit.Id).Single().Value
                                    ),
                                (g, r) => r.ValueExpression = g
                            )
                        }
                    )
                }
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IPrimitiveSet primitives,
            IPropertyValueExpressionParameterSet parameters,
            Neuron unitGranny
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        primitives.Unit
                    ),
                    UnitParameterSet.CreateWithValueAndType(
                        primitives.Of,
                        primitives.Case
                    )
                }
            );

        private static IValueExpressionParameterSet CreateValueExpressionParameterSet(IPropertyValueExpressionParameterSet parameters, Neuron value) =>
            new ValueExpressionParameterSet(
                value,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IPropertyValueExpressionParameterSet parameters, out IPropertyValueExpression result) =>
            this.aggregateParser.TryParse<PropertyValueExpression, IPropertyValueExpression>(
                parameters.Granny,
                PropertyValueExpressionReader.CreateGreatGrannies(
                    this.valueExpressionReader,
                    this.expressionReader,
                    parameters,
                    ensemble,
                    this.primitives
                    ),
                new IGreatGrannyProcess<IPropertyValueExpression>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IValueExpression, IValueExpressionReader, IValueExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                out result
            );
    }
}
