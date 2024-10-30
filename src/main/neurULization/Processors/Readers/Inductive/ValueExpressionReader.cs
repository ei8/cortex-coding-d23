using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ValueExpressionReader : IValueExpressionReader
    {
        private readonly IValueReader valueReader;
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;
        private readonly IAggregateParser aggregateParser;
        private static readonly IGreatGrannyProcess<IValueExpression>[] targets = new IGreatGrannyProcess<IValueExpression>[]
            {
                new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IValueExpression>(
                    ProcessHelper.TryParse
                    ),
                new GreatGrannyProcess<IValue, IValueReader, IValueParameterSet, IValueExpression>(
                    ProcessHelper.TryParse
                    )
            };

    public ValueExpressionReader(
            IValueReader valueReader, 
            IExpressionReader expressionReader, 
            IPrimitiveSet primitives,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(valueReader, nameof(valueReader));
            AssertionConcern.AssertArgumentNotNull(expressionReader, nameof(expressionReader));
            AssertionConcern.AssertArgumentNotNull(primitives, nameof(primitives));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.valueReader = valueReader;
            this.expressionReader = expressionReader;
            this.primitives = primitives;
            this.aggregateParser = aggregateParser;
        }

        private static IGreatGrannyInfoSuperset<IValueExpression> CreateGreatGrannies(
            IValueReader valueReader,
            IExpressionReader expressionReader,
            IValueExpressionParameterSet parameters,
            Ensemble ensemble,
            IPrimitiveSet primitives
        ) =>
            GreatGrannyInfoSuperset<IValueExpression>.Create(
                new GreatGrannyInfoSet<IValueExpression>[] {
                    ProcessHelper.CreateGreatGrannyCandidateSet(
                        ensemble,
                        parameters.Granny,
                        gc => new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IValueExpression>(
                            gc,
                            expressionReader,
                            () => ValueExpressionReader.CreateExpressionParameterSet(primitives, parameters, gc),
                            (g, r) => r.Expression = g
                        ),
                        targets[0]
                    ),
                    new GreatGrannyInfoSet<IValueExpression>(
                        new IGreatGrannyInfo<IValueExpression>[] {
                            new DependentGreatGrannyInfo<IValue, IValueReader, IValueParameterSet, IValueExpression>(
                                valueReader,
                                g => CreateValueParameterSet(
                                    parameters,
                                    ((IExpression) g).Units.GetValueUnitGranniesByTypeId(primitives.Unit.Id).Single().Value
                                    ),
                                (g, r) => r.Value = g
                            )
                        },
                        targets[1]
                    )
                }
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IPrimitiveSet primitives,
            IValueExpressionParameterSet parameters,
            Neuron unitGranny
            ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[] {
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        primitives.Unit
                    ),
                }
            );

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters, Neuron value) =>
            new ValueParameterSet(
                value,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IValueExpressionParameterSet parameters, out IValueExpression result) =>
            this.aggregateParser.TryParse<ValueExpression, IValueExpression>(
                parameters.Granny,
                ValueExpressionReader.CreateGreatGrannies(
                    this.valueReader,
                    this.expressionReader,
                    parameters,
                    ensemble,
                    this.primitives
                ),
                ensemble,
                out result
            );
    }
}
