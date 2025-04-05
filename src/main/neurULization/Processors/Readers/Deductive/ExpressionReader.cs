using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ExpressionReader : IExpressionReader
    {
        private readonly IUnitReader unitReader;
        private readonly IExternalReferenceSet externalReferences;

        public ExpressionReader(IUnitReader unitReader, IExternalReferenceSet externalReferences)
        {
            this.unitReader = unitReader;
            this.externalReferences = externalReferences;
        }

        public bool TryCreateGreatGrannies(
            IExpressionParameterSet parameters,
            Network network,
            IExternalReferenceSet externalReferences,
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

        public IEnumerable<IGrannyQuery> GetQueries(Network network, IExpressionParameterSet parameters) =>
            ExpressionReader.GetQueryByType(this.externalReferences, parameters, this.unitReader);

        private static IEnumerable<IGrannyQuery> GetQueryByType(IExternalReferenceSet externalReferences, IExpressionParameterSet parameters, IUnitReader unitReader)
        {
            IEnumerable<IGrannyQuery> result = null;

            var types = ExpressionReader.GetExpressionTypes(
                (id, isEqual) => parameters.UnitParameters.GetValueUnitParametersByTypeId(id, isEqual).Count(),
                externalReferences
                );

            if (types.Count() == 1)
            {
                if (types.Single().Id == externalReferences.Subordination.Id)
                {
                    result = new[] {
                        new GrannyQuery(
                            new NeuronQuery()
                            {
                                // set Id to values of Dependents (non-Head units)
                                Id = parameters.UnitParameters.GetValueUnitParametersByTypeId(externalReferences.Unit.Id, false)
                                        .Select(dp => dp.Value.Id.ToString()),
                                DirectionValues = DirectionValues.Any,
                                Depth = 4,
                                TraversalDepthPostsynaptic = new[] {
                                    // 4 edges away and should have postsynaptic of unit or values of Head units
                                    new DepthIdsPair {
                                        Depth = 4,
                                        Ids = parameters.UnitParameters.GetValueUnitParametersByTypeId(externalReferences.Unit.Id)
                                            .Select(up => up.Value.Id)
                                            .Concat(
                                                new[] {
                                                    externalReferences.Unit.Id
                                                }
                                            )
                                    },
                                    // 3 edges away and should have postsynaptic of subordination
                                    new DepthIdsPair {
                                        Depth = 3,
                                        Ids = new[] { externalReferences.Subordination.Id }
                                    },
                                    // 2 edges away and should have postsynaptic of non-head unit (eg. direct object)
                                    new DepthIdsPair {
                                        Depth = 2,
                                        Ids = parameters.UnitParameters.GetValueUnitParametersByTypeId(externalReferences.Unit.Id, false)
                                                .Select(up => up.Type.Id)
                                    }
                                }
                            }
                        )
                    };
                }
                else if (types.Single().Id == externalReferences.Simple.Id)
                {
                    result = new IGrannyQuery[] {
                        new GreatGrannyQuery<IUnit, IUnitReader, IUnitParameterSet>(
                            unitReader,
                            (n) => parameters.UnitParameters.Single(u => u.Type.Id == externalReferences.Unit.Id)
                        ),
                        new GrannyQueryBuilder(
                            (n) => new NeuronQuery()
                            {
                                Postsynaptic = new []{
                                    n.Last().Neuron.Id.ToString(),
                                    externalReferences.Simple.Id.ToString()
                                },
                                DirectionValues = DirectionValues.Outbound,
                                Depth = 1
                            }
                        )
                    };
                }
                else if (types.Single().Id == externalReferences.Coordination.Id)
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
            IExternalReferenceSet externalReferences
        )
        {
            var result = new List<Neuron>();

            var headCount = headCountRetriever(externalReferences.Unit.Id, true);
            var dependentCount = headCountRetriever(externalReferences.Unit.Id, false);

            if (headCount > 0)
            {
                if (headCount > 1)
                {
                    result.Add(externalReferences.Coordination);
                }
                else if (headCount == 1 && dependentCount == 0)
                {
                    result.Add(externalReferences.Simple);
                }
                if (dependentCount > 0)
                {
                    result.Add(externalReferences.Subordination);
                }
            }
            else
                throw new InvalidOperationException("Expression must have at least one 'Head' unit.");

            return result.ToArray();
        }

        public bool TryParse(Network network, IExpressionParameterSet parameters, out IExpression result)
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
                this.externalReferences,
                out IExpression tempResult,
                false
            );

            if (tempResult != null && tempResult.Units.Count() == parameters.UnitParameters.Count())
            {
                tempResult.TryParseCore(
                    network,
                    // start from the Head units
                    tempResult.GetValueUnitGranniesByTypeId(this.externalReferences.Unit.Id).Select(u => u.Neuron.Id),
                    new[]
                    {
                        // get the presynaptic via the siblings of the head and subordination
                        new LevelParser(new PresynapticByPostsynapticSibling(
                            tempResult.GetValueUnitGranniesByTypeId(this.externalReferences.Unit.Id, false)
                                .Select(i => i.Neuron.Id)
                                .Concat(
                                    GetExpressionTypes(
                                        (id, isEqual) => parameters.UnitParameters.GetValueUnitParametersByTypeId(id, isEqual).Count(),
                                        this.externalReferences
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
