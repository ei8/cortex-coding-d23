using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ExpressionProcessor : IExpressionProcessor
    {
        private readonly IUnitProcessor unitProcessor;
        private readonly IPrimitiveSet primitives;

        public ExpressionProcessor(IUnitProcessor unitProcessor, IPrimitiveSet primitives)
        {
            this.unitProcessor = unitProcessor;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IExpression>> CreateGreatGrannies(IUnitProcessor unitProcessor, IExpressionParameterSet parameters) =>
            parameters.UnitsParameters.Select(
                u => new IndependentGreatGrannyInfo<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                    unitProcessor,
                    () => u,
                    (g, r) => r.Units.Add(g)
                )
            );

        public IEnumerable<IGrannyQuery> GetQueries(IExpressionParameterSet parameters) =>
            ExpressionProcessor.GetQueryByType(this.primitives, parameters, this.unitProcessor);

        private static IEnumerable<IGrannyQuery> GetQueryByType(IPrimitiveSet primitives, IExpressionParameterSet parameters, IUnitProcessor unitProcessor)
        {
            IEnumerable<IGrannyQuery> result = null;

            var types = ExpressionProcessor.GetExpressionTypes(
                (id, isEqual) => parameters.UnitsParameters.GetValueUnitParametersByTypeId(id, isEqual).Count(),
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
                                Id = parameters.UnitsParameters.GetValueUnitParametersByTypeId(primitives.Unit.Id, false)
                                        .Select(dp => dp.Value.Id.ToString()),
                                DirectionValues = DirectionValues.Any,
                                Depth = 4,
                                TraversalDepthPostsynaptic = new[] {
                                    // 4 edges away and should have postsynaptic of unit or values of Head units
                                    new DepthIdsPair {
                                        Depth = 4,
                                        Ids = parameters.UnitsParameters.GetValueUnitParametersByTypeId(primitives.Unit.Id)
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
                                        Ids = parameters.UnitsParameters.GetValueUnitParametersByTypeId(primitives.Unit.Id, false)
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
                        new GreatGrannyQuery<IUnit, IUnitProcessor, IUnitParameterSet>(
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

        public bool TryParse(Ensemble ensemble, IExpressionParameterSet parameters, out IExpression result)
        {
            result = null;

            new Expression().AggregateTryParse(
                ExpressionProcessor.CreateGreatGrannies(this.unitProcessor, parameters),
                parameters.UnitsParameters.Select(
                    u => new GreatGrannyProcess<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                        ProcessHelper.TryParse
                    )
                ),
                ensemble,
                out IExpression tempResult,
                false
            );

            if (tempResult != null && tempResult.Units.Count() == parameters.UnitsParameters.Count())
            {
                tempResult.TryParseCore(
                    ensemble,
                    // start from the Head units
                    tempResult.Units.GetValueUnitGranniesByTypeId(this.primitives.Unit.Id).Select(u => u.Neuron.Id),
                    new[]
                    {
                        // get the presynaptic via the siblings of the head and subordination
                        new LevelParser(new PresynapticByPostsynapticSibling(
                            tempResult.Units.GetValueUnitGranniesByTypeId(this.primitives.Unit.Id, false)
                                .Select(i => i.Neuron.Id)
                                .Concat(
                                    GetExpressionTypes(
                                        (id, isEqual) => parameters.UnitsParameters.GetValueUnitParametersByTypeId(id, isEqual).Count(),
                                        this.primitives
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
