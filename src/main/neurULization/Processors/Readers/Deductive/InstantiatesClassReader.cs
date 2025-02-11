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
        private readonly IExternalReferenceSet externalReferences;

        public InstantiatesClassReader(IExpressionReader expressionReader, IExternalReferenceSet externalReferences)
        {
            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
        }

        private static IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> CreateGreatGrannies(
            IExpressionReader expressionReader,
            IInstantiatesClassParameterSet parameters,
            IExternalReferenceSet externalReferences
        ) =>
           new IGreatGrannyInfo<IInstantiatesClass>[]
           {
                new IndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                    expressionReader,
                    () => InstantiatesClassReader.CreateSubordinationParameterSet(externalReferences, parameters),
                    (g, r) => r.Class = g.Units.GetValueUnitGranniesByTypeId(externalReferences.DirectObject.Id).Single()
                )
           };

        public IEnumerable<IGrannyQuery> GetQueries(IInstantiatesClassParameterSet parameters) =>
            this.expressionReader.GetQueries(
                InstantiatesClassReader.CreateSubordinationParameterSet(this.externalReferences, parameters)
            );

        private static ExpressionParameterSet CreateSubordinationParameterSet(IExternalReferenceSet externalReferences, IInstantiatesClassParameterSet parameters)
        {
            return new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        externalReferences.Instantiates,
                        externalReferences.Unit
                    ),
                    new UnitParameterSet(
                        parameters.Class,
                        externalReferences.DirectObject
                    )
                }
            );
        }

        public bool TryParse(Network network, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            new InstantiatesClass().AggregateTryParse(
                InstantiatesClassReader.CreateGreatGrannies(
                    this.expressionReader, 
                    parameters,
                    this.externalReferences
                ),
                new IGreatGrannyProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.TryParse
                    )
                },
                network,
                out result
            );
    }
}
