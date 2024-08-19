using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class ExpressionProcessor : IExpressionProcessor
    {
        private readonly IUnitProcessor unitProcessor;

        public ExpressionProcessor(IUnitProcessor unitProcessor)
        {
            this.unitProcessor = unitProcessor;
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
                    ExpressionProcessor.GetExpressionTypes(
                        (id, isEqual) => parameters.UnitsParameters.GetByTypeId(id, isEqual).Count(),
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
                u => new GreatGrannyInfo<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                    unitProcessor,
                    (g) => u,
                    (g, r) => r.Units.Add(g)
                )
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerWriteOptions options, IExpressionParameterSet parameters) =>
            ExpressionProcessor.GetQueryByType(options.Primitives, parameters, this.unitProcessor);

        private static IEnumerable<IGrannyQuery> GetQueryByType(PrimitiveSet primitives, IExpressionParameterSet parameters, IUnitProcessor unitProcessor)
        {
            IEnumerable<IGrannyQuery> result = null;

            var types = ExpressionProcessor.GetExpressionTypes(
                (id, isEqual) => parameters.UnitsParameters.GetByTypeId(id, isEqual).Count(),
                primitives
                );

            if (types.Count() == 1)
            {
                if (types.Single().Id == primitives.Subordination.Id)
                {
                    result = new[] {
                        new GrannyQuery(
                            new NeuronQuery()
                            {
                                // set Id to values of Dependents (non-Head units)
                                Id = parameters.UnitsParameters.GetByTypeId(primitives.Unit.Id, false)
                                        .Select(dp => dp.Value.Id.ToString()),
                                DirectionValues = DirectionValues.Any,
                                Depth = 4,
                                TraversalDepthPostsynaptic = new[] {
                                    // 4 edges away and should have postsynaptic of unit or values of Head units
                                    new DepthIdsPair {
                                        Depth = 4,
                                        Ids = parameters.UnitsParameters.GetByTypeId(primitives.Unit.Id)
                                            .Select(up => up.Value.Id)
                                            .Concat(
                                                new[] {
                                                    primitives.Unit.Id
                                                }
                                            )
                                    },
                                    // 3 edges away and should have postsynaptic of subordination
                                    new DepthIdsPair {
                                        Depth = 3,
                                        Ids = new[] { primitives.Subordination.Id }
                                    },
                                    // 2 edges away and should have postsynaptic of non-head unit (eg. direct object)
                                    new DepthIdsPair {
                                        Depth = 2,
                                        Ids = parameters.UnitsParameters.GetByTypeId(primitives.Unit.Id, false)
                                                .Select(up => up.Type.Id)
                                    }
                                }
                            }
                        )
                    };
                }
                else if (types.Single().Id == primitives.Simple.Id)
                {
                    result = new IGrannyQuery[] {
                        new GrannyQueryInner<IUnit, IUnitProcessor, IUnitParameterSet>(
                            unitProcessor,
                            (n) => parameters.UnitsParameters.Single(u => u.Type.Id == primitives.Unit.Id)
                        ),
                        new GrannyQueryBuilder(
                            (n) => new NeuronQuery()
                            {
                                Postsynaptic = new []{
                                    n.Last().Neuron.Id.ToString(),
                                    primitives.Simple.Id.ToString()
                                },
                                DirectionValues = DirectionValues.Outbound,
                                Depth = 1
                            }
                        )
                    };
                }
                else if (types.Single().Id == primitives.Coordination.Id)
                {
                    throw new NotImplementedException();
                }
            }
            else
                // TODO: Coordination-Subscription
                throw new NotImplementedException();

            return result;
        }

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

        public bool TryParse(Ensemble ensemble, Id23neurULizerWriteOptions options, IExpressionParameterSet parameters, out IExpression result)
        {
            result = null;

            new Expression().AggregateTryParse(
                CreateGreatGrannies(options, parameters),
                parameters.UnitsParameters.Select(
                    u => new GreatGrannyProcess<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                        ProcessHelper.TryParse
                    )
                ),
                ensemble,
                options,
                out IExpression tempResult,
                false
            );


            if (tempResult != null && tempResult.Units.Count() == parameters.UnitsParameters.Count())
            {
                tempResult.TryParseCore(
                    ensemble,
                    // start from the Head units
                    tempResult.Units.GetByTypeId(options.Primitives.Unit.Id).Select(u => u.Neuron.Id),
                    new[]
                    {
                        // get the presynaptic via the siblings of the head and subordination
                        new LevelParser(new PresynapticByPostsynapticSibling(
                            tempResult.Units.GetByTypeId(options.Primitives.Unit.Id, false)
                                .Select(i => i.Neuron.Id)
                                .Concat(
                                    ExpressionProcessor.GetExpressionTypes(
                                        (id, isEqual) => parameters.UnitsParameters.GetByTypeId(id, isEqual).Count(),
                                        options.Primitives
                                    ).Select(t => t.Id)
                                ).ToArray()
                            )
                        )
                    },
                    (n) => tempResult.Neuron = n,
                    ref result
                    );
            }

            return result != null;
        }
    }
}
