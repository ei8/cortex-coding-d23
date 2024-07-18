using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstantiatesClass : IInstantiatesClass
    {
        private readonly IExpression expressionProcessor;

        public InstantiatesClass(IExpression expressionProcessor)
        {
            this.expressionProcessor = expressionProcessor;
        }

        public async Task<IInstantiatesClass> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IInstantiatesClassParameterSet parameters) =>
            await new InstantiatesClass(this.expressionProcessor).AggregateBuildAsync(
                new IInnerProcess<IInstantiatesClass>[]
                {
                    new InnerProcess<IExpression, IExpressionParameterSet, IInstantiatesClass>(
                        this.expressionProcessor,
                        (g) => InstantiatesClass.CreateSubordinationParameterSet(options.Primitives, parameters),
                        (g, r) => r.Class = g.Units.GetByTypeId(options.Primitives.DirectObject.Id).Single(),
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, IInstantiatesClassParameterSet parameters) =>
            this.expressionProcessor.GetQueries(
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

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            (result = new InstantiatesClass(this.expressionProcessor).AggregateTryParse(
                new IInnerProcess<IInstantiatesClass>[]
                {
                    new InnerProcess<IExpression, IExpressionParameterSet, IInstantiatesClass>(
                        this.expressionProcessor,
                        (g) => InstantiatesClass.CreateSubordinationParameterSet(options.Primitives, parameters),
                        (g, r) => r.Class = g.Units.GetByTypeId(options.Primitives.DirectObject.Id).Single(),
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            )) != null;

        public IUnit Class { get; set; }

        public Neuron Neuron { get; set; }
    }
}
