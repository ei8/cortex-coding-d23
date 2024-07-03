using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ei8.Cortex.Coding.d23.Queries;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstantiatesClass : IInstantiatesClass
    {
        public async Task<IInstantiatesClass> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters) =>
            await new InstantiatesClass().AggregateBuildAsync(
                new IInnerProcess<InstantiatesClass>[]
                {
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, InstantiatesClass>(
                        (g) => InstantiatesClass.CreateSubordinationParameterSet(primitives, parameters),
                        (g, r) => r.Class = g.Units.GetByTypeId(primitives.DirectObject.Id),
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                },
                ensemble,
                primitives,
                (n, r) => r.Neuron = n
            );

        public IEnumerable<IGrannyQuery> GetQueries(IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters) =>
            new Expression().GetQueries(
                primitives,
                InstantiatesClass.CreateSubordinationParameterSet(primitives, parameters)
                );

        private static ExpressionParameterSet CreateSubordinationParameterSet(IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters)
        {
            return new ExpressionParameterSet(
                new IUnitParameterSet[]
                {
                    new UnitParameterSet(
                        primitives.Instantiates,
                        primitives.Unit
                    ),
                    new UnitParameterSet(
                        parameters.Class,
                        primitives.DirectObject
                    )
                },
                parameters.EnsembleRepository,
                parameters.UserId
            );
        }

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            (result = new InstantiatesClass().AggregateTryParse(
                new IInnerProcess<InstantiatesClass>[]
                {
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, InstantiatesClass>(
                        (g) => InstantiatesClass.CreateSubordinationParameterSet(primitives, parameters),
                        (g, r) => r.Class = g.Units.GetByTypeId(primitives.DirectObject.Id),
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                primitives,
                (n, r) => r.Neuron = n
            )) != null;

        public IUnit Class { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
