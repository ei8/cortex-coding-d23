using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
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
        private readonly IPrimitiveSet primitives;

        public ExpressionProcessor(
            IUnitProcessor unitProcessor, 
            Readers.Deductive.IExpressionProcessor readProcessor, 
            IPrimitiveSet primitives
        )
        {
            this.unitProcessor = unitProcessor;
            this.readProcessor = readProcessor;
            this.primitives = primitives;
        }

        public async Task<IExpression> BuildAsync(Ensemble ensemble, IExpressionParameterSet parameters) =>
            await new Expression().AggregateBuildAsync(
                ExpressionProcessor.CreateGreatGrannies(
                    this.unitProcessor,
                    parameters
                ),
                parameters.UnitsParameters.Select(
                    u => new GreatGrannyProcessAsync<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                        ProcessHelper.ObtainAsync
                    )
                ),
                ensemble,
                () => ensemble.Obtain(Neuron.CreateTransient(null, null, null)),
                (r) =>
                    // concat applicable expression types
                    GetExpressionTypes(
                        (id, isEqual) => parameters.UnitsParameters.GetValueUnitParametersByTypeId(id, isEqual).Count(),
                        this.primitives
                    )
                    .Select(et => ensemble.Obtain(et))
                    .Concat(
                        // with Units in result
                        r.Units.Select(u => u.Neuron)
                    )
            );

        private static IEnumerable<IGreatGrannyInfo<IExpression>> CreateGreatGrannies(
            IUnitProcessor unitProcessor,
            IExpressionParameterSet parameters
        ) =>
            parameters.UnitsParameters.Select(
                u => new IndependentGreatGrannyInfo<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                    unitProcessor,
                    () => u,
                    (g, r) => r.Units.Add(g)
                )
            );

        internal static IEnumerable<Neuron> GetExpressionTypes(
            Func<Guid, bool, int> headCountRetriever,
            IPrimitiveSet primitives
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
