using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstantiatesClass : IInstantiatesClass
    {
        public async Task<IInstantiatesClass> BuildAsync(Ensemble ensemble, Id23neurULizationOptions options, IInstantiatesClassParameterSet parameters) =>
            await new InstantiatesClass().AggregateBuildAsync(
                new IInnerProcess<InstantiatesClass>[]
                {
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, InstantiatesClass>(
                        (g) => InstantiatesClass.CreateSubordinationParameterSet(options.Primitives, parameters),
                        (g, r) => r.Class = g.Units.GetByTypeId(options.Primitives.DirectObject.Id).Single(),
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizationOptions options, IInstantiatesClassParameterSet parameters) =>
            new Expression().GetQueries(
                options,
                InstantiatesClass.CreateSubordinationParameterSet(options.Primitives, parameters)
                );

        private static ExpressionParameterSet CreateSubordinationParameterSet(PrimitiveSet primitives, IInstantiatesClassParameterSet parameters)
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
                }
            );
        }

        public bool TryParse(Ensemble ensemble, Id23neurULizationOptions options, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            (result = new InstantiatesClass().AggregateTryParse(
                new IInnerProcess<InstantiatesClass>[]
                {
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, InstantiatesClass>(
                        (g) => InstantiatesClass.CreateSubordinationParameterSet(options.Primitives, parameters),
                        (g, r) => r.Class = g.Units.GetByTypeId(options.Primitives.DirectObject.Id).Single(),
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            )) != null;

        public IUnit Class { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
