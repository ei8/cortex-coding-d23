﻿using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstantiatesClassProcessor : IInstantiatesClassProcessor
    {
        private readonly IExpressionProcessor expressionProcessor;

        public InstantiatesClassProcessor(IExpressionProcessor expressionProcessor)
        {
            this.expressionProcessor = expressionProcessor;
        }

        private static IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> CreateGreatGrannies(IExpressionProcessor expressionProcessor, Id23neurULizerReadOptions options, IInstantiatesClassParameterSet parameters) =>
           new IGreatGrannyInfo<IInstantiatesClass>[]
           {
                new IndependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                    expressionProcessor,
                    () => CreateSubordinationParameterSet(options.Primitives, parameters),
                    (g, r) => r.Class = g.Units.GetValueUnitGranniesByTypeId(options.Primitives.DirectObject.Id).Single()
                )
           };

        private static ExpressionParameterSet CreateSubordinationParameterSet(PrimitiveSet primitives, IInstantiatesClassParameterSet parameters) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        primitives.Instantiates,
                        primitives.Unit
                    ),
                    UnitParameterSet.CreateWithValueAndType(
                        parameters.Class,
                        primitives.DirectObject
                    )
                }
            );

        public bool TryParse(Ensemble ensemble, Id23neurULizerReadOptions options, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            new InstantiatesClass().AggregateTryParse(
                parameters.Granny,
                CreateGreatGrannies(expressionProcessor, options, parameters),
                new IGreatGrannyProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                options,
                1,
                out result
            );
    }
}