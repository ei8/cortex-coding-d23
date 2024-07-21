﻿using ei8.Cortex.Coding.d23.neurULization;
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
                this.CreateInnerProcesses(options, parameters),
                new IInnerProcessTargetAsync<IInstantiatesClass>[]
                {
                    new InnerProcessTargetAsync<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options
            );

        private IEnumerable<IInnerProcess<IInstantiatesClass>> CreateInnerProcesses(Id23neurULizerOptions options, IInstantiatesClassParameterSet parameters) =>
           new IInnerProcess<IInstantiatesClass>[]
           {
                new InnerProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                    this.expressionProcessor,
                    (g) => InstantiatesClassProcessor.CreateSubordinationParameterSet(options.Primitives, parameters),
                    (g, r) => r.Class = g.Units.GetByTypeId(options.Primitives.DirectObject.Id).Single()
                )
           };

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
                this.CreateInnerProcesses(options, parameters),
                new IInnerProcessTarget<IInstantiatesClass>[]
                {
                    new InnerProcessTarget<IExpression, IExpressionProcessor, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                options,
                out result
            );
    }
}
