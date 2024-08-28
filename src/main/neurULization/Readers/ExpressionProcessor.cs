using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Writers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public class ExpressionProcessor : IExpressionProcessor
    {
        private readonly IUnitProcessor unitProcessor;

        public ExpressionProcessor(IUnitProcessor unitProcessor)
        {
            this.unitProcessor = unitProcessor;
        }

        private static IEnumerable<IGreatGrannyInfo<IExpression>> CreateGreatGrannies(
            IUnitProcessor unitProcessor,
            IExpressionParameterSet parameters,
            Ensemble ensemble
            ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => parameters.UnitParameters.Where(up => up.Granny == null).Select(
                    up => new IndependentGreatGrannyInfo<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                        unitProcessor,
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
                    up => new IndependentGreatGrannyInfo<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                        unitProcessor,
                        () => UnitParameterSet.CreateWithGrannyAndType(
                                up.Granny,
                                up.Type
                            ),
                        (g, r) => r.Units.Add(g)
                    )
                )
            );

        public bool TryParse(Ensemble ensemble, Id23neurULizerReadOptions options, IExpressionParameterSet parameters, out IExpression result) =>
            new Expression().AggregateTryParse(
                parameters.Granny,
                ExpressionProcessor.CreateGreatGrannies(
                    this.unitProcessor,
                    parameters,
                    ensemble
                ),
                new IGreatGrannyProcess<IExpression>[]
                {
                    new GreatGrannyProcess<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                options,
                parameters.UnitParameters.Count(),
                out result
            );
    }
}
