using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.neurULization.Processors;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using System.Collections.Generic;
using System.Linq;
using System;
using ei8.Cortex.Library.Common;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public abstract class ExpressionReaderBase<TExpressionParameterSet> :
        ILesserGrannyReader<IExpression, TExpressionParameterSet>
        where TExpressionParameterSet : IExpressionParameterSet
    {
        private readonly IUnitReader unitReader;
        private readonly IMirrorSet mirrors;

        public ExpressionReaderBase(IUnitReader unitReader, IMirrorSet mirrors)
        {
            this.unitReader = unitReader;
            this.mirrors = mirrors;
        }

        public bool TryCreateGreatGrannies(
            TExpressionParameterSet parameters,
            Network network,
            IMirrorSet mirrors,
            out IEnumerable<IGreatGrannyInfo<IExpression>> result
        ) => this.TryCreateGreatGranniesCore(
            delegate (out bool bResult) {
                bResult = true;
                var coreBResult = true;
                var coreResult = parameters.UnitParameters.Select(
                    u => new IndependentGreatGrannyInfo<IUnit, IUnitReader, IUnitParameterSet, IExpression>(
                        unitReader,
                        () => u,
                        (g, r) => r.Units.Add(g)
                    )
                );
                bResult = coreBResult;
                return coreResult;
            },
            out result
        );

        public IEnumerable<IGrannyQuery> GetQueries(Network network, TExpressionParameterSet parameters) =>
            GetQueryByType(this.mirrors, parameters, this.unitReader);

        private static IEnumerable<IGrannyQuery> GetQueryByType(IMirrorSet mirrors, TExpressionParameterSet parameters, IUnitReader unitReader)
        {
            IEnumerable<IGrannyQuery> result = null;

            var types = ExpressionReader.GetExpressionTypes(
                (id, isEqual) => parameters.UnitParameters.GetValueUnitParametersByTypeId(id, isEqual).Count(),
                mirrors
                );

            // TODO: Coordination-Subordination
            if (types.HasFlag(ExpressionType.Subordination) && types.HasFlag(ExpressionType.Coordination))
                throw new NotImplementedException();
            else if (types.HasFlag(ExpressionType.Subordination))
            {
                result = new[] {
                    new GrannyQuery(
                        new NeuronQuery()
                        {
                            // set Id to values of Dependents (non-Head units)
                            Id = parameters.UnitParameters.GetValueUnitParametersByTypeId(mirrors.Unit.Id, false)
                                    .Select(dp => dp.Value.Id.ToString()),
                            DirectionValues = DirectionValues.Any,
                            Depth = 4,
                            TraversalDepthPostsynaptic = new[] {
                                // 4 edges away and should have postsynaptic of unit or values of Head units
                                new DepthIdsPair {
                                    Depth = 4,
                                    Ids = parameters.UnitParameters.GetValueUnitParametersByTypeId(mirrors.Unit.Id)
                                        .Select(up => up.Value.Id)
                                        .Concat(
                                            new[] {
                                                mirrors.Unit.Id
                                            }
                                        )
                                },
                                // 2 edges away and should have postsynaptic of non-head unit (eg. direct object)
                                new DepthIdsPair {
                                    Depth = 2,
                                    Ids = parameters.UnitParameters.GetValueUnitParametersByTypeId(mirrors.Unit.Id, false)
                                            .Select(up => up.Type.Id)
                                }
                            }
                        }
                    )
                };
            }
            else if (types.HasFlag(ExpressionType.Simple))
            {
                result = new IGrannyQuery[] {
                    new GreatGrannyQuery<IUnit, IUnitReader, IUnitParameterSet>(
                        unitReader,
                        (n) => parameters.UnitParameters.Single(u => u.Type.Id == mirrors.Unit.Id)
                    ),
                    new GrannyQueryBuilder(
                        (n) => new NeuronQuery()
                        {
                            Postsynaptic = new[] { n.Last().Neuron.Id.ToString() },
                            DirectionValues = DirectionValues.Outbound,
                            Depth = 1
                        }
                    )
                };
            }
            else if (types.HasFlag(ExpressionType.Coordination))
            {
                throw new NotImplementedException();
            }

            return result;
        }

        internal static ExpressionType GetExpressionTypes(
            Func<Guid, bool, int> headCountRetriever,
            IMirrorSet mirrors
        )
        {
            var result = ExpressionType.NotSet;

            var headCount = headCountRetriever(mirrors.Unit.Id, true);
            var dependentCount = headCountRetriever(mirrors.Unit.Id, false);

            if (headCount > 0)
            {
                if (headCount > 1)
                {
                    result |= ExpressionType.Coordination;
                }
                else if (headCount == 1 && dependentCount == 0)
                {
                    result |= ExpressionType.Simple;
                }
                if (dependentCount > 0)
                {
                    result |= ExpressionType.Subordination;
                }
            }
            else
                throw new InvalidOperationException("Expression must have at least one 'Head' unit.");

            return result;
        }

        public bool TryParse(Network network, TExpressionParameterSet parameters, out IExpression result)
        {
            result = null;

            this.TryParseAggregate(
                () => new Expression(),
                parameters,
                parameters.UnitParameters.Select(
                    u => new GreatGrannyProcess<IUnit, IUnitReader, IUnitParameterSet, IExpression>(
                        ProcessHelper.TryParse
                    )
                ),
                network,
                this.mirrors,
                out IExpression tempResult,
                false
            );

            if (tempResult != null && tempResult.Units.Count() == parameters.UnitParameters.Count())
            {
                tempResult.TryParseCore(
                    network,
                    // start from the Head units
                    tempResult.GetValueUnitGranniesByTypeId(this.mirrors.Unit.Id).Select(u => u.Neuron.Id),
                    new[]
                    {
                        // get the presynaptic via the siblings of the head and subordination
                        new LevelParser(new PresynapticByPostsynapticSibling(
                            tempResult.GetValueUnitGranniesByTypeId(this.mirrors.Unit.Id, false)
                                .Select(i => i.Neuron.Id)
                                .ToArray()
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
