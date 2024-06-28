using ei8.Cortex.Coding.d23.Queries;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ValueExpression : IValueExpression
    {
        public async Task<IValueExpression> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IValueExpressionParameterSet parameters)
        {
            var result = new ValueExpression();
            result.Value = await new Value().ObtainAsync(
                ensemble,
                primitives,
                ValueExpression.CreateValueParameterSet(parameters)
                );
            result.Expression = await new Expression().ObtainAsync(
                ensemble,
                primitives,
                new ExpressionParameterSet(
                    new[]
                    {
                        new UnitParameterSet(
                            result.Value.Neuron,
                            primitives.Unit
                        )
                    },
                    parameters.EnsembleRepository, 
                    parameters.UserId
                )
            );
            result.Neuron = result.Expression.Neuron;
            return result;
        }

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.MatchingNeuronProperty,
                parameters.EnsembleRepository,
                parameters.UserId
            );

        public IEnumerable<IGrannyQuery> GetQueries(IPrimitiveSet primitives, IValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryParser<IValueParameterSet>(
                    ValueExpression.CreateValueParameterSet(parameters),
                    (ps) => new Value().GetQueries(
                            primitives,
                            ps
                        ),
                    (Ensemble e, IPrimitiveSet prs, IValueParameterSet ps, out IGranny r) =>
                        ((IValue) new Value()).TryParseGranny(
                            e,
                            prs,
                            ps,
                            out r
                            )
                ),
                // TODO: use result of preceding granny OR USE Expression.GetQueries similar to InstantiatesClass
                //
                // new GrannyQueryBuilder(
                //    (n) => new NeuronQuery()
                //    {
                //        Id = parameters.MatchingNeuronProperty == InstantiationMatchingNeuronProperty.Id ?
                //            new[] { parameters.Value.Id.ToString() } :
                //            Array.Empty<string>(),
                //        TagContains = parameters.MatchingNeuronProperty == InstantiationMatchingNeuronProperty.Tag ?
                //            new[] { parameters.Value.Tag } :
                //            Array.Empty<string>(),
                //        DirectionValues = DirectionValues.Outbound,
                //        Depth = 1,
                //        TraversalDepthPostsynaptic = new[] {
                //            // 1 edge away and should have postsynaptic of previous granny
                //            new DepthIdsPair {
                //                Depth = 1,
                //                Ids = new[] { n.Id }
                //            },
                //        }
                //    }
                //)
            };

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IValueExpressionParameterSet parameters, out IValueExpression result)
        {
            throw new NotImplementedException();
        }

        public IValue Value { get; private set; }

        public IExpression Expression { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
