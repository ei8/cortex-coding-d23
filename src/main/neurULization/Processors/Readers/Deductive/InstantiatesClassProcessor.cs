using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
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

        private static IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> CreateGreatGrannies(
            IExpressionProcessor expressionProcessor,
            IInstantiatesClassParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
           new IGreatGrannyInfo<IInstantiatesClass>[]
           {
                new IndependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                    expressionProcessor,
                    () => InstantiatesClassProcessor.CreateSubordinationParameterSet(primitives, parameters),
                    (g, r) => r.Class = g.Units.GetValueUnitGranniesByTypeId(primitives.DirectObject.Id).Single()
                )
           };

        public IEnumerable<IGrannyQuery> GetQueries(IInstantiatesClassParameterSet parameters) =>
            this.expressionProcessor.GetQueries(
                InstantiatesClassProcessor.CreateSubordinationParameterSet(this.primitives, parameters)
            );

        private static ExpressionParameterSet CreateSubordinationParameterSet(IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters)
        {
            return new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        primitives.Instantiates,
                        primitives.Unit
                    ),
                    new UnitParameterSet(
                        parameters.Class,
                        primitives.DirectObject
                    )
                }
            );
        }

        public bool TryParse(Ensemble ensemble, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            new InstantiatesClass().AggregateTryParse(
                InstantiatesClassProcessor.CreateGreatGrannies(
                    this.expressionProcessor, 
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
                out result
            );
    }
}
