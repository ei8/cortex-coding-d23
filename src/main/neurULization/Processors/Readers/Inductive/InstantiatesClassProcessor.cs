using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstantiatesClassProcessor : IInstantiatesClassProcessor
    {
        private readonly IExpressionProcessor expressionProcessor;
        private readonly IPrimitiveSet primitives;

        public InstantiatesClassProcessor(IExpressionProcessor expressionProcessor, IPrimitiveSet primitives)
        {
            this.expressionProcessor = expressionProcessor;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> CreateGreatGrannies(IExpressionProcessor expressionProcessor, IInstantiatesClassParameterSet parameters, IPrimitiveSet primitives) =>
           new IGreatGrannyInfo<IInstantiatesClass>[]
           {
                new IndependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                    expressionProcessor,
                    () => CreateSubordinationParameterSet(primitives, parameters),
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
                InstantiatesClassProcessor.CreateGreatGrannies(
                    expressionProcessor, 
                    parameters, 
                    this.primitives
                ),
                new IGreatGrannyProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                1,
                out result
            );
    }
}
