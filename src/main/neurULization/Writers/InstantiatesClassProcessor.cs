using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class InstantiatesClassProcessor : IInstantiatesClassProcessor
    {
        private readonly IExpressionProcessor expressionProcessor;

        public InstantiatesClassProcessor(IExpressionProcessor expressionProcessor)
        {
            this.expressionProcessor = expressionProcessor;
        }

        public async Task<IInstantiatesClass> BuildAsync(Ensemble ensemble, Id23neurULizerWriteOptions options, IInstantiatesClassParameterSet parameters) =>
            await new InstantiatesClass().AggregateBuildAsync(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyWriteProcessAsync<IInstantiatesClass>[]
                {
                    new GreatGrannyWriteProcessAsync<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options
            );

        private IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> CreateGreatGrannies(Id23neurULizerWriteOptions options, IInstantiatesClassParameterSet parameters) =>
           new IGreatGrannyInfo<IInstantiatesClass>[]
           {
                new GreatGrannyWriteInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                    expressionProcessor,
                    (g) => CreateSubordinationParameterSet(options.Primitives, parameters),
                    (g, r) => r.Class = g.Units.GetByTypeId(options.Primitives.DirectObject.Id).Single()
                )
           };

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerWriteOptions options, IInstantiatesClassParameterSet parameters) =>
            expressionProcessor.GetQueries(
                options,
                CreateSubordinationParameterSet(options.Primitives, parameters)
                );

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

        public bool TryParse(Ensemble ensemble, Id23neurULizerWriteOptions options, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            new InstantiatesClass().AggregateTryParse(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyWriteProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyWriteProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                options,
                out result
            );
    }
}
