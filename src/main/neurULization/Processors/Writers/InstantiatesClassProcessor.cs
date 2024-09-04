using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class InstantiatesClassProcessor : IInstantiatesClassProcessor
    {
        private readonly IExpressionProcessor expressionProcessor;
        private readonly Readers.Deductive.IInstantiatesClassProcessor readProcessor;

        public InstantiatesClassProcessor(
            IExpressionProcessor expressionProcessor,
            Readers.Deductive.IInstantiatesClassProcessor readProcessor
            )
        {
            this.expressionProcessor = expressionProcessor;
            this.readProcessor = readProcessor;
        }

        public async Task<IInstantiatesClass> BuildAsync(Ensemble ensemble, Id23neurULizerWriteOptions options, IInstantiatesClassParameterSet parameters) =>
            await new InstantiatesClass().AggregateBuildAsync(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcessAsync<IInstantiatesClass>[]
                {
                    new GreatGrannyProcessAsync<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options
            );

        private IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> CreateGreatGrannies(Id23neurULizerWriteOptions options, IInstantiatesClassParameterSet parameters) =>
           new IGreatGrannyInfo<IInstantiatesClass>[]
           {
                new IndependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                    expressionProcessor,
                    () => CreateSubordinationParameterSet(options.Primitives, parameters),
                    (g, r) => r.Class = g.Units.GetValueUnitGranniesByTypeId(options.Primitives.DirectObject.Id).Single()
                )
           };

        private static ExpressionParameterSet CreateSubordinationParameterSet(PrimitiveSet primitives, IInstantiatesClassParameterSet parameters)
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
