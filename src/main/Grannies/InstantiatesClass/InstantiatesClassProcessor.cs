using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstantiatesClassProcessor : IInstantiatesClassProcessor
    {
        private readonly IExpressionProcessor expressionProcessor;

        public InstantiatesClassProcessor(IExpressionProcessor expressionProcessor)
        {
            this.expressionProcessor = expressionProcessor;
        }

        public async Task<IInstantiatesClass> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IInstantiatesClassParameterSet parameters) =>
            await new InstantiatesClass().AggregateBuildAsync(
                new[]
                {
                    new InnerProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                        this.expressionProcessor,
                        (g) => InstantiatesClassProcessor.CreateSubordinationParameterSet(options.Primitives, parameters),
                        (g, r) => r.Class = g.Units.GetByTypeId(options.Primitives.DirectObject.Id).Single(),
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                },
                ensemble,
                options
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, IInstantiatesClassParameterSet parameters) =>
            this.expressionProcessor.GetQueries(
                options,
                InstantiatesClassProcessor.CreateSubordinationParameterSet(options.Primitives, parameters)
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

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            new InstantiatesClass().AggregateTryParse(
                new[]
                {
                    new InnerProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                        this.expressionProcessor,
                        (g) => InstantiatesClassProcessor.CreateSubordinationParameterSet(options.Primitives, parameters),
                        (g, r) => r.Class = g.Units.GetByTypeId(options.Primitives.DirectObject.Id).Single(),
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                out result
            );
    }
}
