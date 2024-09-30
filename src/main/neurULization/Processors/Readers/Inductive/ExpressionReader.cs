using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ExpressionReader : IExpressionReader
    {
        private readonly IUnitReader unitReader;

        public ExpressionReader(IUnitReader unitReader)
        {
            this.unitReader = unitReader;
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
                    up => new IndependentGreatGrannyInfo<IUnit, IUnitReader, IUnitParameterSet, IExpression>(
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
                    up => new IndependentGreatGrannyInfo<IUnit, IUnitReader, IUnitParameterSet, IExpression>(
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
            new Expression().AggregateTryParse(
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
