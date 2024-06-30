using ei8.Cortex.Coding.d23.Queries;
using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Expression : IExpression
    {
        public async Task<IExpression> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IExpressionParameterSet parameters)
        {
            var result = new Expression();
            
            var units = new List<IUnit>();
            foreach (var dp in parameters.UnitsParameters)
            {
                var unit = await new Unit().ObtainAsync(
                    new ObtainParameters(
                        ensemble,
                        primitives,
                        parameters.EnsembleRepository,
                        parameters.UserId
                    ),
                    new UnitParameterSet(
                        dp.Value,
                        dp.Type
                        )
                    );
                units.Add(unit);
            }
            result.Units = units;
            result.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null));

            var types = Expression.GetExpressionTypes(parameters, primitives);
            types.ToList().ForEach(t => ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, ensemble.Obtain(t).Id)));
            units.ForEach(u => ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, u.Neuron.Id)));

            return result;
        }

        public IEnumerable<IGrannyQuery> GetQueries(IPrimitiveSet primitives, IExpressionParameterSet parameters) =>
            Expression.GetQueryByType(primitives, parameters);

        private static IEnumerable<IGrannyQuery> GetQueryByType(IPrimitiveSet primitives, IExpressionParameterSet parameters)
        {
            IEnumerable<IGrannyQuery> result = null;
            
            var types = Expression.GetExpressionTypes(parameters, primitives);

            if (types.Count() == 1)
            {
                if (types.Single().Id == primitives.Subordination.Id)
                {
                    result = new[] {
                        new GrannyQuery( 
                            new NeuronQuery()
                            {
                                // set Id to values of Dependents (non-Head units)
                                Id = parameters.UnitsParameters
                                        .Where(up => up.Type.Id != primitives.Unit.Id)
                                        .Select(dp => dp.Value.Id.ToString()),
                                DirectionValues = DirectionValues.Any,
                                Depth = 4,
                                TraversalDepthPostsynaptic = new[] {
                                    // 4 edges away and should have postsynaptic of unit or values of Head units
                                    new DepthIdsPair {
                                        Depth = 4,
                                        Ids = parameters.UnitsParameters
                                            .Where(up => up.Type.Id == primitives.Unit.Id)
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
                                    // 2 edges away and should have postsynaptic of direct object
                                    new DepthIdsPair {
                                        Depth = 2,
                                        Ids = new[] {
                                            primitives.DirectObject.Id
                                        }
                                    }
                                }
                            }
                        )
                    };
                }
                else if (types.Single().Id == primitives.Simple.Id)
                {
                    result = new IGrannyQuery[] {
                        new GrannyQueryParser<IUnitParameterSet>(
                            (n) => parameters.UnitsParameters.Single(u => u.Type.Id == primitives.Unit.Id),
                            (ps) => new Unit().GetQueries(
                                    primitives,
                                    ps
                                ),
                            (Ensemble e, IPrimitiveSet prs, IUnitParameterSet ps, out IGranny r) =>
                                ((IUnit) new Unit()).TryParseGranny(
                                    e,
                                    prs,
                                    ps,
                                    out r
                                    )
                        ),
                        new GrannyQueryBuilder(
                            (n) => new NeuronQuery()
                            {
                                Postsynaptic = new []{ 
                                    n.Id.ToString(),
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

        private static IEnumerable<Neuron> GetExpressionTypes(IExpressionParameterSet expressionParameters, IPrimitiveSet primitives)
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

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IExpressionParameterSet parameters, out IExpression result)
        {
            result = null;

            var tempResult = new Expression();

            var units = new List<IUnit>();
            foreach (var dp in parameters.UnitsParameters)
            {
                if (new Unit().TryParse(ensemble, primitives, dp, out IUnit ide))
                    units.Add(ide);
            }

            if (units.Count == parameters.UnitsParameters.Count())
            {
                tempResult.Units = units;

                this.TryParseCore(
                    parameters,
                    ensemble,
                    tempResult,
                    // start from the Head units
                    tempResult.Units
                        .Where(u => u.Type.Id == primitives.Unit.Id)
                        .Select(u => u.Neuron.Id),
                    new[]
                    {
                        // get the presynaptic via the siblings of the head and subordination
                        new LevelParser(new PresynapticBySibling(
                            units
                                .Where(u => u.Type.Id != primitives.Unit.Id)
                                .Select(i => i.Neuron.Id)
                                .Concat(Expression.GetExpressionTypes(parameters, primitives).Select(t => t.Id)).ToArray()
                            ))
                    },
                    (n) => tempResult.Neuron = n,
                    ref result
                    );
            }

            return result != null;
        }

        public IEnumerable<IUnit> Units { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
