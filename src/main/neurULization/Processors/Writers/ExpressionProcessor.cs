using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ExpressionProcessor : IExpressionProcessor
    {
        private readonly IUnitProcessor unitProcessor;
        private readonly Readers.Deductive.IExpressionProcessor readProcessor;
        
        public ExpressionProcessor(IUnitProcessor unitProcessor, Readers.Deductive.IExpressionProcessor readProcessor)
        {
            this.unitProcessor = unitProcessor;
            this.readProcessor = readProcessor;
        }

        public async Task<IExpression> BuildAsync(Ensemble ensemble, Id23neurULizerWriteOptions options, IExpressionParameterSet parameters) =>
            await new Expression().AggregateBuildAsync(
                CreateGreatGrannies(options, parameters),
                parameters.UnitsParameters.Select(
                    u => new GreatGrannyProcessAsync<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                        ProcessHelper.ObtainAsync
                    )
                ),
                ensemble,
                options,
                () => ensemble.Obtain(Neuron.CreateTransient(null, null, null)),
                (r) =>
                    // concat applicable expression types
                    GetExpressionTypes(
                        (id, isEqual) => parameters.UnitsParameters.GetValueUnitParametersByTypeId(id, isEqual).Count(),
                        options.Primitives
                    )
                    .Select(et => ensemble.Obtain(et))
                    .Concat(
                        // with Units in result
                        r.Units.Select(u => u.Neuron)
                    )
            );

        private IEnumerable<IGreatGrannyInfo<IExpression>> CreateGreatGrannies(Id23neurULizerWriteOptions options, IExpressionParameterSet parameters) =>
            parameters.UnitsParameters.Select(
                u => new IndependentGreatGrannyInfo<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                    unitProcessor,
                    () => u,
                    (g, r) => r.Units.Add(g)
                )
            );

        internal static IEnumerable<Neuron> GetExpressionTypes(
            Func<Guid, bool, int> headCountRetriever,
            PrimitiveSet primitives
            )
        {
            var result = new List<Neuron>();

            var headCount = headCountRetriever(primitives.Unit.Id, true);
            var dependentCount = headCountRetriever(primitives.Unit.Id, false);

            if (headCount > 0)
            {
                if (headCount > 1)
                {
                    result.Add(primitives.Coordination);
                }
                else if (headCount == 1 && dependentCount == 0)
                {
                    result.Add(primitives.Simple);
                }
                if (dependentCount > 0)
                {
                    result.Add(primitives.Subordination);
                }
            }
            else
                throw new InvalidOperationException("Expression must have at least one 'Head' unit.");

            return result.ToArray();
        }

        public Readers.Deductive.IGrannyReadProcessor<IExpression, IExpressionParameterSet> ReadProcessor => this.readProcessor;
    }
}
