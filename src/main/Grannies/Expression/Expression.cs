﻿using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Expression : IExpression
    {
        public async Task<IExpression> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IExpressionParameterSet parameters)
        {
            var result = new Expression();
            var subordination = ensemble.Obtain(primitives.Subordination);
            
            var units = new List<IUnit>();
            foreach (var dp in parameters.UnitsParameters)
            {
                var unit = await new Unit().ObtainAsync(
                    ensemble,
                    primitives,
                    new UnitParameterSet(
                        dp.Value,
                        dp.Type
                        ),
                    parameters.EnsembleRepository,
                    parameters.UserId
                    );
                units.Add(unit);
            }
            result.Units = units;
            result.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, subordination.Id));
            units.ForEach(u => ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, u.Neuron.Id)));

            return result;
        }

        public IEnumerable<GrannyQuery> GetQueries(IPrimitiveSet primitives, IExpressionParameterSet parameters) =>
            new[] {
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
                                Ids = new[] {
                                    Expression.GetExpressionTypeId(parameters, primitives)
                                }
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

        private static Guid GetExpressionTypeId(IExpressionParameterSet expressionParameters, IPrimitiveSet primitives)
        {
            Guid result = Guid.Empty;

            var headCount = expressionParameters.UnitsParameters.Count(up => up.Type.Id == primitives.Unit.Id);
            var dependentCount = expressionParameters.UnitsParameters.Count() - headCount;

            // coordination
            if (headCount > 1)
            {
                if (dependentCount > 0)
                {
                    //TODO: result = primitives.CoordinationSubordination.Id
                    throw new NotImplementedException();
                }
                else
                {
                    // TODO: result = primitives.Coordination.Id
                    throw new NotImplementedException();
                }
            }
            // subordination
            else if (headCount == 1)
            {
                if (dependentCount > 0)
                    result = primitives.Subordination.Id;
                else
                    // TODO: result = primitives.Simple.Id
                    throw new NotImplementedException();
            }
            else
                throw new InvalidOperationException("Expression must have at least one 'Head' unit.");

            return result;
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
                                .Concat(new[] { primitives.Subordination.Id }).ToArray()
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
