using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ExpressionReader : IExpressionReader
    {
        private readonly IUnitReader unitReader;
        private readonly IMirrorSet mirrors;
        private readonly IAggregateParser aggregateParser;
        private static readonly IGreatGrannyProcess<IExpression> target = new GreatGrannyProcess<IUnit, IUnitReader, IUnitParameterSet, IExpression>(
            ProcessHelper.TryParse
        );

        public ExpressionReader(
            IUnitReader unitReader,
            IMirrorSet mirrors,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(unitReader, nameof(unitReader));
            AssertionConcern.AssertArgumentNotNull(mirrors, nameof(mirrors));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.unitReader = unitReader;
            this.mirrors = mirrors;
            this.aggregateParser = aggregateParser;
        }

        public bool TryCreateGreatGrannies(
            IExpressionParameterSet parameters, 
            Network network, 
            IMirrorSet mirrors, 
            out IGreatGrannyInfoSuperset<IExpression> result
        ) => this.TryCreateGreatGranniesCore(
            delegate (out bool bResult) {
                bResult = true;
                var coreBResult = true;
                var coreResult = ProcessHelper.CreateGreatGrannyInfoSuperset(
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
                    ProcessHelper.CreateGreatGrannyInfoSuperset(
                        network,
                        parameters.Granny,
                        parameters.UnitParameters.Where(up => up.Granny != null),
                        (gc, up) => new InductiveIndependentGreatGrannyInfo<IUnit, IUnitReader, IUnitParameterSet, IExpression>(
                            up.Granny,
                            unitReader,
                            () => UnitParameterSet.CreateWithGrannyAndType(
                                up.Granny,
                                up.Type
                            ),
                            (g, r) => r.Units.Add(g)
                        ),
                        ExpressionReader.target
                    )
                );
                bResult = coreBResult;
                return coreResult;
            },
            out result
        );

        public bool TryParse(Network network, IExpressionParameterSet parameters, out IExpression result) =>
            this.aggregateParser.TryParse<Expression, IExpression, IExpressionParameterSet>(
                parameters.Granny,
                this,
                parameters,
                network,
                this.mirrors,
                out result
            );
    }
}
