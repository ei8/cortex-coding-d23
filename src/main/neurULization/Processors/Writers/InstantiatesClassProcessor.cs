using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class InstantiatesClassProcessor : IInstantiatesClassProcessor
    {
        private readonly IExpressionProcessor expressionProcessor;
        private readonly Readers.Deductive.IInstantiatesClassProcessor readProcessor;
        private readonly IPrimitiveSet primitives;

        public InstantiatesClassProcessor(
            IExpressionProcessor expressionProcessor,
            Readers.Deductive.IInstantiatesClassProcessor readProcessor,
            IPrimitiveSet primitives
            )
        {
            this.expressionProcessor = expressionProcessor;
            this.readProcessor = readProcessor;
            this.primitives = primitives;
        }

        public IInstantiatesClass Build(Ensemble ensemble, IInstantiatesClassParameterSet parameters) =>
            new InstantiatesClass().AggregateBuild(
                InstantiatesClassProcessor.CreateGreatGrannies(
                    this.expressionProcessor, 
                    parameters, 
                    this.primitives
                ),
                new IGreatGrannyProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.ParseBuild
                    )
                },
                ensemble
            );

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

        public IGrannyReadProcessor<IInstantiatesClass, IInstantiatesClassParameterSet> ReadProcessor => this.readProcessor;
    }
}
