﻿using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstantiatesClassReader : IInstantiatesClassReader
    {
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;

        public InstantiatesClassReader(IExpressionReader expressionReader, IPrimitiveSet primitives)
        {
            this.expressionReader = expressionReader;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> CreateGreatGrannies(IExpressionReader expressionReader, IInstantiatesClassParameterSet parameters, IPrimitiveSet primitives) =>
           new IGreatGrannyInfo<IInstantiatesClass>[]
           {
                new IndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                    expressionReader,
                    () => InstantiatesClassReader.CreateSubordinationParameterSet(primitives, parameters),
                    (g, r) => r.Class = g.Units.GetValueUnitGranniesByTypeId(primitives.DirectObject.Id).Single()
                )
           };

        private static ExpressionParameterSet CreateSubordinationParameterSet(IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters) =>
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

        public bool TryParse(Ensemble ensemble, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            new InstantiatesClass().AggregateTryParse(
                parameters.Granny,
                InstantiatesClassReader.CreateGreatGrannies(
                    this.expressionReader, 
                    parameters, 
                    this.primitives
                ),
                new IGreatGrannyProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                1,
                out result
            );
    }
}
