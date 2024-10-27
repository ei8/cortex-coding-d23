using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ExpressionReader : IExpressionReader
    {
        private readonly IUnitReader unitReader;
        private readonly IAggregateParser aggregateParser;

        public ExpressionReader(IUnitReader unitReader, IAggregateParser aggregateParser)
        {
            AssertionConcern.AssertArgumentNotNull(unitReader, nameof(unitReader));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.unitReader = unitReader;
            this.aggregateParser = aggregateParser;
        }

        private static IEnumerable<IGreatGrannyInfo<IExpression>> CreateGreatGrannies(
            IUnitReader unitReader,
            IExpressionParameterSet parameters,
            Ensemble ensemble
        ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => parameters.UnitParameters.Where(up => up.Granny == null).Select(
                    up => new InductiveIndependentGreatGrannyInfo<IUnit, IUnitReader, IUnitParameterSet, IExpression>(
                        gc,
                        unitReader,
                        () => UnitParameterSet.Create(
                                gc,
                                up.Value,
                                up.Type
                            ),
                        (g, r) => r.Units.Add(g)
                    )
                )
            ).Concat(
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
                )
            );

        public bool TryParse(Ensemble ensemble, IExpressionParameterSet parameters, out IExpression result) =>
            this.aggregateParser.TryParse<Expression, IExpression>(
                parameters.Granny,
                ExpressionReader.CreateGreatGrannies(
                    this.unitReader,
                    parameters,
                    ensemble
                ),
                new IGreatGrannyProcess<IExpression>[]
                {
                    new GreatGrannyProcess<IUnit, IUnitReader, IUnitParameterSet, IExpression>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                parameters.UnitParameters.Count(),
                out result
            );
    }
}
