using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ExpressionReader : IExpressionReader
    {
        private readonly IUnitReader unitReader;
        private readonly IAggregateParser aggregateParser;
        private static readonly IGreatGrannyProcess<IExpression> target = new GreatGrannyProcess<IUnit, IUnitReader, IUnitParameterSet, IExpression>(
                ProcessHelper.TryParse
            );

        public ExpressionReader(IUnitReader unitReader, IAggregateParser aggregateParser)
        {
            AssertionConcern.AssertArgumentNotNull(unitReader, nameof(unitReader));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.unitReader = unitReader;
            this.aggregateParser = aggregateParser;
        }

        private static IGreatGrannyInfoSuperset<IExpression> CreateGreatGrannies(
            IUnitReader unitReader,
            IExpressionParameterSet parameters,
            Network network
        ) =>
            ProcessHelper.CreateGreatGrannyCandidateSets(
                network,
                parameters.Granny,
                parameters.UnitParameters.Where(up => up.Granny == null),
                (gc, up) => new InductiveIndependentGreatGrannyInfo<IUnit, IUnitReader, IUnitParameterSet, IExpression>(
                    gc,
                    unitReader,
                    () => UnitParameterSet.Create(
                        gc,
                        up.Value,
                        up.Type
                    ),
                    (g, r) => r.Units.Add(g)
                ),
                ExpressionReader.target
            ).Concat(
                new GreatGrannyInfoSet<IExpression>(
                    parameters.UnitParameters.Where(up => up.Granny != null).Select(
                        up => new InductiveIndependentGreatGrannyInfo<IUnit, IUnitReader, IUnitParameterSet, IExpression>(
                            up.Granny,
                            unitReader,
                            () => UnitParameterSet.CreateWithGrannyAndType(
                                    up.Granny,
                                    up.Type
                                ),
                            (g, r) => r.Units.Add(g)
                        )
                    ),
                    ExpressionReader.target
                ).AsSuperset()
            );

        public bool TryParse(Network network, IExpressionParameterSet parameters, out IExpression result) =>
            this.aggregateParser.TryParse<Expression, IExpression>(
                parameters.Granny,
                ExpressionReader.CreateGreatGrannies(
                    this.unitReader,
                    parameters,
                    network
                ),
                network,
                out result
            );
    }
}
