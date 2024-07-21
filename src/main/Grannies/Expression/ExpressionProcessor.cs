using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ExpressionProcessor : IExpressionProcessor
    {
        private readonly IUnitProcessor unitProcessor;

        public ExpressionProcessor(IUnitProcessor unitProcessor)
        {
            this.unitProcessor = unitProcessor;
        }

        public async Task<IExpression> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IExpressionParameterSet parameters) =>
            await new Expression().AggregateBuildAsync(
                this.CreateInnerProcesses(options, parameters),
                parameters.UnitsParameters.Select(
                    u => new InnerProcessTargetAsync<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                        ProcessHelper.ObtainAsync
                    )
                ),
                ensemble,
                options,
                () => ensemble.Obtain(Neuron.CreateTransient(null, null, null)),
                (r) =>
                    // concat applicable expression types
                    ExpressionProcessor.GetExpressionTypes(parameters, options.Primitives).Select(et => ensemble.Obtain(et)).Concat(
                        // with Units in result
                        r.Units.Select(u => u.Neuron)
                    )
            );

        private IEnumerable<IInnerProcess<IExpression>> CreateInnerProcesses(Id23neurULizerOptions options, IExpressionParameterSet parameters) =>
            parameters.UnitsParameters.Select(
                u => new InnerProcess<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
                    this.unitProcessor,
                    (g) => u,
                    (g, r) => r.Units.Add(g)
                )
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, IExpressionParameterSet parameters) =>
            this.GetQueryByType(options.Primitives, parameters);

        private IEnumerable<IGrannyQuery> GetQueryByType(PrimitiveSet primitives, IExpressionParameterSet parameters)
        {
            IEnumerable<IGrannyQuery> result = null;

            var types = ExpressionProcessor.GetExpressionTypes(parameters, primitives);

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
                            this.unitProcessor,
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

        private static IEnumerable<Neuron> GetExpressionTypes(IExpressionParameterSet expressionParameters, PrimitiveSet primitives)
        {
            var result = new List<Neuron>();

            var headCount = expressionParameters.UnitsParameters.Count(up => up.Type.Id == primitives.Unit.Id);
            var dependentCount = expressionParameters.UnitsParameters.Count() - headCount;

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

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IExpressionParameterSet parameters, out IExpression result)
        {
            result = null;

            new Expression().AggregateTryParse(
                this.CreateInnerProcesses(options, parameters),
                parameters.UnitsParameters.Select(
                    u => new InnerProcessTarget<IUnit, IUnitProcessor, IUnitParameterSet, IExpression>(
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
                this.TryParseCore(
                    parameters,
                    ensemble,
                    tempResult,
                    // start from the Head units
                    tempResult.Units.GetByTypeId(options.Primitives.Unit.Id).Select(u => u.Neuron.Id),
                    new[]
                    {
                        // get the presynaptic via the siblings of the head and subordination
                        new LevelParser(new PresynapticBySibling(
                            tempResult.Units.GetByTypeId(options.Primitives.Unit.Id, false)
                                .Select(i => i.Neuron.Id)
                                .Concat(
                                    ExpressionProcessor.GetExpressionTypes(
                                        parameters,
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
