using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstantiatesClassReader : IInstantiatesClassReader
    {
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;

        public InstantiatesClassReader(IExpressionReader expressionReader, IPrimitiveSet primitives)
        {
            this.expressionReader = expressionReader;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> CreateGreatGrannies(
            IExpressionReader expressionReader,
            IInstantiatesClassParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
           new IGreatGrannyInfo<IInstantiatesClass>[]
           {
                new IndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                    expressionReader,
                    () => InstantiatesClassReader.CreateSubordinationParameterSet(primitives, parameters),
                    (g, r) => r.Class = g.Units.GetValueUnitGranniesByTypeId(primitives.DirectObject.Id).Single()
                )
           };

        public IEnumerable<IGrannyQuery> GetQueries(IInstantiatesClassParameterSet parameters) =>
            this.expressionReader.GetQueries(
                InstantiatesClassReader.CreateSubordinationParameterSet(this.primitives, parameters)
            );

        private static ExpressionParameterSet CreateSubordinationParameterSet(IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters)
        {
            return new ExpressionParameterSet(
                new[]
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

        public bool TryParse(Ensemble ensemble, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            new InstantiatesClass().AggregateTryParse(
                InstantiatesClassReader.CreateGreatGrannies(
                    this.expressionReader, 
                    parameters,
                    this.primitives
                ),
                new IGreatGrannyProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                out result
            );
    }
}
