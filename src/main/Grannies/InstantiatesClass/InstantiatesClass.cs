using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstantiatesClass : IInstantiatesClass
    {
        public async Task<IInstantiatesClass> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters)
        {
            var result = new InstantiatesClass();

            var subordination = await new Expression().BuildAsync(
                ensemble,
                primitives,
                InstantiatesClass.CreateSubordinationParameterSet(primitives, parameters)
                );

            result.Class = subordination.Units.Single(u => u.Type.Id == primitives.DirectObject.Id);
            result.Neuron = subordination.Neuron;

            return result;
        }

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

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result)
        {
            result = null;

            var tempResult = new InstantiatesClass();

            if (new Expression().TryParse(
                ensemble,
                primitives,
                InstantiatesClass.CreateSubordinationParameterSet(primitives, parameters),
                out IExpression subordination
                )
                )
            {
                tempResult.Class = subordination.Units.Single(u => u.Type.Id == primitives.DirectObject.Id);
                tempResult.Neuron = subordination.Neuron;
                result = tempResult;
            }

            return result != null;
        }

        public IUnit Class { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
